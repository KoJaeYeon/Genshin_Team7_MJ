using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndriusAttack : IPattern
{
    private Andrius _andrius;
    private Animator _animator;
    private Transform _player;

    private WaitForSeconds _jumpDelay = new WaitForSeconds(8f);
    private WaitForSeconds _chargeDelay = new WaitForSeconds(10f);

    private float _angle;
    private float _distance;

    private bool _isJump = true;
    private bool _isCharge = true;

    public void InitializePattern(Andrius andrius)
    {
        if(_andrius == null)
        {
            _andrius = andrius;
            _animator = _andrius.GetComponent<Animator>();
        }

        if(_andrius.Paralyzation <= 0f)
        {
            _andrius.JumpBack = true;
            _andrius.Turn = true;
            _andrius.State.ChangeState(BossState.Idle);
        }

        _player = _andrius.PlayerTransform;
    }
    public void UpdatePattern()
    {
        CalculateAngle();
        CalculateDistance();
        Attack(_angle, _distance);
    }

    public void ExitPattern()
    {
        _angle = 0f;
        _distance = 0f; 
    }

    private void Attack(float angle, float distance)
    {
        Back(angle, distance);
        Turn(angle);

        if (!_andrius.MoveStop)
        {
            if (distance <= 10f)
            {
                MeleeAttack(angle);
            }
            else if(distance <= 20f && _isJump)
            {
                _isJump = false;
                _andrius.StartCoroutine(JumpDelay());
                _andrius.State.ChangeState(BossState.Jump);
            }
            else if(distance <= 100f && _isCharge)
            {
                _isCharge = false;
                _andrius.StartCoroutine(ChargeDelay());
                _andrius.State.ChangeState(BossState.Charge);
            }
            else if(distance > 100f)
            {
                _andrius.State.ChangeState(BossState.Move);
            }
            else
            {
                _andrius.State.ChangeState(BossState.Howl);
            }
        }
    }

    private void CalculateAngle()
    {
        Vector3 targetDirection = (_player.position - _andrius.transform.position).normalized;

        Vector3 andriusForward = _andrius.transform.forward;

        _angle = Vector3.SignedAngle(andriusForward, targetDirection, Vector3.up);
    }

    private void CalculateDistance()
    {
        float distance = Vector3.Distance(_player.position, _andrius.transform.position);

        _distance = distance;
    }

    private void Back(float angle, float distance)
    {
        if (!_andrius.Turn || !(angle > -90f && angle < 90f))
        {
            return;
        }

        if(distance <= 5.5f && _andrius.JumpBack)
        {
            _andrius.JumpBack = false;

            Vector3 targetDirection = _player.position - _andrius.transform.position;

            targetDirection.y = 0f;

            float targetAngle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;

            _andrius.transform.rotation = Quaternion.Euler(0, targetAngle, 0);

            _animator.SetTrigger("JumpBack");
        }

    }

    private void Turn(float angle)
    {
        if (!_andrius.JumpBack)
        {
            return;
        }

        if(angle >= 120f && _andrius.Turn)
        {
            _animator.SetTrigger("TurnRight");
            _andrius.Turn = false;
        }
        else if(angle <= -120f && _andrius.Turn)
        {
            _animator.SetTrigger("TurnLeft");
            _andrius.Turn = false;
        }
    }

    private void MeleeAttack(float angle)
    {
        float absAngle = Mathf.Abs(angle);

        if (absAngle <= 60f)
        {
            int random = Random.Range(0, 3);

            switch (random)
            {
                case 0:
                    _andrius.State.ChangeState(BossState.Claw);
                    break;
                case 1:
                    _andrius.State.ChangeState(BossState.Claw);
                    break;
                case 2:
                    _andrius.State.ChangeState(BossState.Stamp);
                    break;
            }
        }
        else if (absAngle <= 120f)
        {
            _andrius.State.ChangeState(BossState.Drift);
        }
        else
            return;
    }
    
    private IEnumerator JumpDelay()
    {
        yield return _jumpDelay;

        _isJump = true;
    }

    private IEnumerator ChargeDelay()
    {
        yield return _chargeDelay;

        _isCharge = true;
    }
}
