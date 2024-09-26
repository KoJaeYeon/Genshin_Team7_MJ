using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Andrius_Charge : Andrius_AttackController, IAndriusChargeEvent
{
    public Andrius_Charge(Andrius andrius) : base(andrius)
    {
        AndriusEventManager.Instance.RegisterChargeEvent(this);

        _rotationSpeed = _chargeData.RotationSpeed;
        _maxAngle = _chargeData.MaxAngle;
        _timer = new WaitForSeconds(_chargeData.ChargeTime);
    }

    private Action _onCollider;
    private Action _offCollider;
    private WaitForSeconds _timer;

    private bool _isHit;
    private bool _isRun = true;
    private bool _isCharge = true;

    private float _rotationSpeed;
    private float _maxAngle;
    private float _currentAngle;

    private Vector3 _movePosition;

    public override void StateEnter()
    {
        bool paralyzation = Paralyzation();

        if (paralyzation)
        {
            return;
        }

        if (!_isCharge)
        {
            _state.ChangeState(BossState.Howl);

            return;
        }

        _movePosition = _playerTransform.position;

        _onCollider.Invoke();

        _animator.SetBool(_run, true);

        _andrius.StartCoroutine(ChargeCoolTime());
    }

    public override void StateFixedUpdate()
    {
        Vector3 targetDirection = (_movePosition - _andrius.transform.position).normalized;

        targetDirection.y = 0f;

        Vector3 forward = _andrius.transform.forward;

        _currentAngle = Vector3.Angle(forward, targetDirection);

        if(_currentAngle > _maxAngle && _isRun)
        {
            _isRun = false;

            _offCollider.Invoke();

            ScramAnimation();
        }

        if (_isRun)
        {
            Rotation(targetDirection);
        }
    }

    public override void StateExit()
    {
        _isRun = true;

        _currentAngle = 0f;
    }

    private void ScramAnimation()
    {
        float angle = CalculateSignedAngle();

        _animator.SetBool(_run, false);

        if(angle > 0f)
        {
            _animator.SetTrigger(_scramRight);
            _andrius.StartCoroutine(Next());
        }
        else if(angle < 0f)
        {
            _animator.SetTrigger(_scramLeft);
            _andrius.StartCoroutine(Next());
        }
        else
        {
            _animator.SetTrigger(_scramLeft);
            _andrius.StartCoroutine(Next());
        }
    }

    private IEnumerator Next()
    {
        yield return _timer;

        NextPattern();
    }

    private IEnumerator ChargeCoolTime()
    {
        _isCharge = false;

        yield return new WaitForSeconds(_attackData.GetData(AttackDataList.ChargeDelay));

        _isCharge = true;
    }

    private void Rotation(Vector3 targetDirection)
    {
        float atan2 = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(0f, atan2, 0f);

        _andrius.transform.rotation = Quaternion.Slerp(_andrius.transform.rotation, rotation, _rotationSpeed * Time.fixedDeltaTime);
    }

    public void OnChargeColliderEvent(Action callBack)
    {
        _onCollider += callBack;
    }

    public void OffChargeColliderEvent(Action callBack)
    {
        _offCollider += callBack;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !_isHit)
        {
            _isHit = true;

            Character player = other.gameObject.GetComponentInChildren<Character>();

            if (player != null)
            {
                player.TakeDamage(_andrius.GetAtk() * _chargeData.SkillDamage);
            }
        }
    }

}