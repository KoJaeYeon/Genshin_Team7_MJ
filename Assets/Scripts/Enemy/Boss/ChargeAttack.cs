using System;
using UnityEngine;

public class ChargeAttack : IPattern, IAndriusChargeEvent
{
    private Andrius _andrius;
    private Animator _animator;
    private Transform _player;
    private Rigidbody _rigidBody;
    private Action _onCollider;
    private Action _offCollider;

    private float _currentAngle;
    private float _distance;
    private float _rotationSpeed = 10f;

    private Vector3 _targetPos;
    private Vector3 _movePos;

    public ChargeAttack()
    {
        AndriusEventManager.Instance.RegisterChargeEvent(this);
    }

    public void InitializePattern(Andrius andrius)
    {
        if(_andrius == null)
        {
            _andrius = andrius;
            _animator = _andrius.GetComponent<Animator>();  
            _rigidBody = _andrius.GetComponent<Rigidbody>();
        }

        _player = _andrius.PlayerTransform;
        _targetPos = (_player.position - _andrius.transform.position).normalized;
        _movePos = _player.position + _targetPos * 2f;
        _onCollider.Invoke();
        _animator.SetBool("isRun", true); 
    }

    public void UpdatePattern()
    {
        _distance = Vector3.Distance(_andrius.transform.position, _movePos);

        if(_distance > 2f)
        {
            Rotation();
        }
        else
        {
            _animator.SetBool("isRun", false);
            _offCollider.Invoke();
            SelectAnimation();
            _rigidBody.velocity = Vector3.zero; 
        }
    }

    public void ExitPattern() { }

    private void Rotation()
    {
        Vector3 rotateDirection = _movePos - _andrius.transform.position;

        rotateDirection.y = 0f;

        float angle = Mathf.Atan2(rotateDirection.x, rotateDirection.z) * Mathf.Rad2Deg;

        _currentAngle = angle;

        Quaternion rotation = Quaternion.Euler(0f, angle, 0f);

        _andrius.transform.rotation = Quaternion.Slerp(_andrius.transform.rotation, rotation, _rotationSpeed * Time.fixedDeltaTime);
    }

    private void SelectAnimation()
    {
        if (_currentAngle > 0 && _currentAngle <= 180 && !_andrius.IsRunStop)
        {
            _andrius.IsRunStop = true;
            _animator.Play("ScramR");
            _andrius.State.ChangeState(BossState.Attack);
        }
        else if (_currentAngle < 0 && _currentAngle >= -180 && !_andrius.IsRunStop)
        {
            _andrius.IsRunStop = true;
            _animator.Play("ScramL");
            _andrius.State.ChangeState(BossState.Attack);
        }
        else
        {
            _andrius.IsRunStop = true;
            _animator.Play("ScramL");
            _andrius.State.ChangeState(BossState.Attack);
        }
    }

    public void OnChargeColliderEvent(Action callBack)
    {
        _onCollider += callBack;
    }

    public void OffChargeColliderEvent(Action callBack)
    {
        _offCollider += callBack;
    }
}
