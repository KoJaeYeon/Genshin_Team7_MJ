using Cinemachine.Utility;
using System;
using System.Collections;
using UnityEngine;

public class ChargeAttack : IPattern, IAndriusChargeEvent
{
    private Andrius _andrius;
    private Animator _animator;
    private Transform _player;
    private Rigidbody _rigidBody;
    private Action _onCollider;
    private Action _offCollider;

    private WaitForSeconds _timer = new WaitForSeconds(1.0f);

    private float _currentAngle;
    private float _distance;
    private float _rotationSpeed = 10f;

    private bool isRun = true;

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

        Vector3 targetDirection = _movePos - _andrius.transform.position;

        targetDirection.y = 0f;

        Vector3 forwardDirection = _andrius.transform.forward;

        _currentAngle = Vector3.Angle(forwardDirection, targetDirection);

        if(_currentAngle > 120f && isRun)
        {
            isRun = false;

            _offCollider?.Invoke();
            ScramAnimation();
        }

        if (isRun)
        {
            Rotation(targetDirection);
        }
        
    }

    public void ExitPattern()
    {
        isRun = true;
        _currentAngle = 0f;
    }

    private void Rotation(Vector3 targetDirection)
    {
        float angle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(0f, angle, 0f);

        _andrius.transform.rotation = Quaternion.Slerp(_andrius.transform.rotation, rotation, _rotationSpeed * Time.fixedDeltaTime);
    }
    private void ScramAnimation()
    {
        Vector3 targetDirection = (_player.position - _andrius.transform.position).normalized;

        Vector3 andriusForward = _andrius.transform.forward;

        float angle = Vector3.SignedAngle(andriusForward, targetDirection, Vector3.up);

        _animator.SetBool("isRun", false);

        if (angle > 0f)
        {
            _animator.SetTrigger("ScramRight");
            _andrius.StartCoroutine(ChangeAttack());
        }
        else if (angle < 0f)
        {
            _animator.SetTrigger("ScramLeft");
            _andrius.StartCoroutine(ChangeAttack());
        }
        else
        {
            _animator.SetTrigger("ScramLeft");
            _andrius.StartCoroutine(ChangeAttack());
        }
            
    }

    private IEnumerator ChangeAttack()
    {
        yield return _timer;

        _andrius.State.ChangeState(BossState.Attack);   
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
