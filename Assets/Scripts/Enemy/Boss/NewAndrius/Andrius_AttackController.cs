using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Andrius_AttackController : Andrius_State
{
    public Andrius_AttackController(Andrius andrius) : base(andrius) { }

    protected float _currentAngle;
    protected float _currentDistance;
    
    protected void NextPattern()
    {
        _currentAngle = CalculateAngle();

        _currentDistance = CalculateDistance();

        if (!CanAction())
        {
            return;
        }

        IsBack(_currentDistance);
        IsTurn(_currentAngle);

        if (ChangePattern(_currentDistance,
            _attackData.GetData(AttackDataList.MeleeDistance)))
        {
            MeleeAttack(_currentAngle);
        }
        else
        {
            JumpAndCharge();
        }
    }

    private bool CanAction()
    {
        return !_andrius.IsAction;
    }

    private bool ChangePattern(float currentDistance, float dataDistance)
    {
        return currentDistance <= dataDistance;  
    }

    private void MeleeAttack(float angle)
    {
        if(angle < _attackData.GetData(AttackDataList.MeleeAngle))
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
        else
        {
            _state.ChangeState(BossState.Drift);
        }
    }

    private void JumpAndCharge()
    {
        int random = Random.Range(0, 2);

        switch (random)
        {
            case 0:
                _state.ChangeState(BossState.Jump);
                break;
            case 1:
                _state.ChangeState(BossState.Charge);
                break;
        }
    }

    private void IsBack(float distance)
    {
        if(distance <= _attackData.GetData(AttackDataList.Back_Distance) && CanAction())
        {
            _state.ChangeState(BossState.Back);
        }
    }

    private void IsTurn(float angle)
    {
        if (angle >= _attackData.GetData(AttackDataList.Turn_rightAngle) && CanAction())
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
