using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Andrius_Jump : Andrius_AttackController
{
    public Andrius_Jump(Andrius andrius) : base(andrius) { }

    private Vector3 _endPosition;
    private bool _isJumping = true;

    public override void StateEnter()
    {
        if (_andrius.Paralyzation <= 0)
        {
            _state.ChangeState(BossState.Idle);

            return;
        }

        if (!_isJumping)
        {
            _state.ChangeState(BossState.Howl);

            return;
        }

        _andrius.IsAction = true;

        _animator.SetTrigger(_jump);

        _rigidbody.isKinematic = true;

        _andrius.StartCoroutine(JumpCoolTime());
    }
    public override void StateFixedUpdate()
    {
        NextPattern();

        var animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if(animatorStateInfo.IsName("Jump"))
        {
            _endPosition = _playerTransform.position;

            if (animatorStateInfo.normalizedTime > 0.1f && animatorStateInfo.normalizedTime < 0.3f)
            {
                Vector3 targetDirection = (_endPosition - _andrius.transform.position).normalized;

                Rotation(targetDirection);
                Move(targetDirection);
            }
        }
    }

    public override void StateExit()
    {
        _rigidbody.isKinematic = false;
    }

    private void Rotation(Vector3 targetDirection)
    {
        targetDirection.y = 0f;

        float angle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(0f, angle, 0f);

        _andrius.transform.rotation = Quaternion.Slerp(_andrius.transform.rotation, rotation, _jumpData.RotationSpeed * Time.deltaTime);
    }

    private void Move(Vector3 targetDirection)
    {
        Vector3 velocity = targetDirection * _jumpData.MoveSpeed * Time.fixedDeltaTime;

        if(Vector3.Distance(_andrius.transform.position, _endPosition) > velocity.magnitude)
        {
            _andrius.transform.Translate(velocity, Space.World);
        }
        else
        {
            _andrius.transform.position = _endPosition;
        }
    }

    private IEnumerator JumpCoolTime()
    {
        _isJumping = false;

        yield return new WaitForSeconds(_attackData.GetData(AttackDataList.JumpDelay));

        _isJumping = true;
    }
}