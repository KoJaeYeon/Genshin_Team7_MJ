using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndriusAttack : IPattern
{
    private Andrius _andrius;
    private Animator _animator;
    private Transform _player;
    private AndriusAttackData _attackData;

    private WaitForSeconds _jumpDelay;
    private WaitForSeconds _chargeDelay;

    #region Value
    private float _meleeDistance;
    private float _jumpDistance;
    private float _chargeDistance;
    private float _moveDistance;
    private float _turn_Rightangle;
    private float _turn_Leftangle;
    private float _back_Rightangle;
    private float _back_Leftangle;
    private float _back_Distance;
    private float _meleeAngle;
    private float _driftAngle;
    #endregion

    public AndriusAttack()
    {
        GetData();
    }

    private float _angle;
    private float _distance;

    private bool _isJump = true;
    private bool _isCharge = true;

    private void GetData()
    {
        _attackData = EnemyCSVLoder.Instance.GetAndriusCSVData<AndriusAttackData>("AndriusAttackData");
        _jumpDelay = new WaitForSeconds(_attackData.GetData(AttackDataList.JumpDelay));
        _chargeDelay = new WaitForSeconds(_attackData.GetData(AttackDataList.ChargeDelay));
        _meleeDistance = _attackData.GetData(AttackDataList.MeleeDistance);
        _jumpDistance = _attackData.GetData(AttackDataList.JumpDistance);
        _chargeDistance = _attackData.GetData(AttackDataList.ChargeDistance);
        _moveDistance = _attackData.GetData(AttackDataList.MoveDistance);
        _turn_Rightangle = _attackData.GetData(AttackDataList.Turn_rightAngle);
        _turn_Leftangle = _attackData.GetData(AttackDataList.Turn_leftAngle);
        _back_Rightangle = _attackData.GetData(AttackDataList.Back_rightAngle);
        _back_Leftangle = _attackData.GetData(AttackDataList.Back_leftAngle);
        _back_Distance = _attackData.GetData(AttackDataList.Back_Distance);
        _meleeAngle = _attackData.GetData(AttackDataList.MeleeAngle);
        _driftAngle = _attackData.GetData(AttackDataList.DriftAngle);
    }

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
            if (distance <= _meleeDistance)
            {
                MeleeAttack(angle);
            }
            else if(distance <= _jumpDistance && _isJump)
            {
                _isJump = false;
                _andrius.StartCoroutine(JumpDelay());
                _andrius.State.ChangeState(BossState.Jump);
            }
            else if(distance <= _chargeDistance && _isCharge)
            {
                _isCharge = false;
                _andrius.StartCoroutine(ChargeDelay());
                _andrius.State.ChangeState(BossState.Charge);
            }
            else if(distance > _moveDistance)
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
        if (!_andrius.Turn || !(angle > _back_Leftangle && angle < _back_Rightangle))
        {
            return;
        }

        if(distance <= _back_Distance && _andrius.JumpBack)
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

        if(angle >= _turn_Rightangle && _andrius.Turn)
        {
            _animator.SetTrigger("TurnRight");
            _andrius.Turn = false;
        }
        else if(angle <= _turn_Leftangle && _andrius.Turn)
        {
            _animator.SetTrigger("TurnLeft");
            _andrius.Turn = false;
        }
    }

    private void MeleeAttack(float angle)
    {
        float absAngle = Mathf.Abs(angle);

        if (absAngle <= _meleeAngle)
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
        else if (absAngle <= _driftAngle)
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
