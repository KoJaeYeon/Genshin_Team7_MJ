using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Andrius_AttackController : Andrius_State
{
    public Andrius_AttackController(Andrius andrius) : base(andrius) { }

    private float _currentAngle;
    private float _currentDistance;
    
    protected void NextPattern()
    {
        _currentAngle = CalculateAngle();

        _currentDistance = CalculateDistance();

        if (!CanAction())
        {
            return;
        }

        TurnAndBack(_currentDistance, _currentAngle);

        if (_currentDistance <= 10f)
        {
            MeleeAttack(_currentAngle);
        }
        else if(_currentDistance <= 18f)
        {
            _state.ChangeState(BossState.Jump);
        }
        else
        {
            _state.ChangeState(BossState.Charge);
        }
    }

    protected void Pattern()
    {
        _currentAngle = CalculateAngle();

        _currentDistance = CalculateDistance();

        if (!CanAction())
        {
            return;
        }

        if (_currentDistance <= 10f)
        {
            MeleeAttack(_currentAngle);
        }
        else if (_currentDistance <= 18f)
        {
            _state.ChangeState(BossState.Jump);
        }
        else
        {
            _state.ChangeState(BossState.Charge);
        }
    }

    private bool CanAction()
    {
        return !_andrius.IsAction;
    }

    private void MeleeAttack(float angle)
    {
        if(angle <= 60f)
        {
            int random = Random.Range(0, 3);

            switch(random)
            {
                case 0:
                    _state.ChangeState(BossState.Claw);
                    break;
                case 1:
                    _state.ChangeState(BossState.Claw);
                    break;
                case 2:
                    _state.ChangeState(BossState.Stamp);
                    break;
            }
        }
        else if(angle < 120f)
        {
            _state.ChangeState(BossState.Drift);
        }
        else
        {
            _state.ChangeState(BossState.Turn);
        }
    }

    private void TurnAndBack(float distance, float angle)
    {
        if(angle >= 120f && CanAction())
        {
            _state.ChangeState(BossState.Turn);
        }
        else if(angle < 90f && CanAction())
        {
            if(distance <= 5.5f)
            {
                _state.ChangeState(BossState.Back);
            }
        }
    }

    protected bool Paralyzation()
    {
        if (_andrius.Paralyzation <= 0)
        {
            _andrius.IsAction = false;

            _state.ChangeState(BossState.Idle);

            return true;
        }

        return false;
    }

    private void IsBack(float distance, float angle)
    {
        if(distance <= 5.5f && CanAction())
        {

            _state.ChangeState(BossState.Back);
        }
    }

    private void IsTurn(float angle)
    {
        if (angle >= 120f && CanAction())
        {
            _state.ChangeState(BossState.Turn);
        }
    }

    public float CalculateAngle()
    {
        Vector3 targetDirection = (_playerTransform.position - _andrius.transform.position).normalized;

        targetDirection.y = 0f;

        Vector3 forward = _andrius.transform.forward;

        float angle = Vector3.Angle(forward, targetDirection);

        return angle;
    }

    public float CalculateSignedAngle()
    {
        Vector3 targetDirection = (_playerTransform.position - _andrius.transform.position).normalized;

        targetDirection.y = 0f;

        Vector3 forward = _andrius.transform.forward;

        float angle = Vector3.SignedAngle(forward, targetDirection, Vector3.up);

        return angle;
    }

    public float CalculateDistance()
    {
        float distance = Vector3.Distance(_andrius.transform.position, _playerTransform.position);

        return distance;
    }

}
