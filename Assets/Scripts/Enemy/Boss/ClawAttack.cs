using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawAttack : IPattern
{
    private enum Claw
    {
        Claw,
        Claw_Drift
    }

    private Andrius _andrius;
    private Animator _animator;
    private Transform _player;

    private int _random;
    private float _angle;

    public void InitializePattern(Andrius andrius)
    {
        if(_andrius == null)
        {
            _andrius = andrius;
            _animator = _andrius.GetComponent<Animator>();
        }

        _player = _andrius.PlayerTransform;
        RandomClawAttack();
    }

    public void UpdatePattern() { }
    
    public void ExitPattern() { }
    
    private float CalculateAngle()
    {
        Vector3 targetDirection = (_player.position - _andrius.transform.position).normalized;

        Vector3 andriusForward = _andrius.transform.forward;

        float angle = Vector3.SignedAngle(andriusForward, targetDirection, Vector3.up);

        return angle;
    }

    private void RandomClawAttack()
    {
        _random = Random.Range(0, 2);
        _angle = CalculateAngle();

        if(_angle == 0)
        {
            _andrius.State.ChangeState(BossState.Attack);
            return;
        }

        switch (_random)
        {
            case (int)Claw.Claw:
                TriggerClawAnimation(_angle, "ClawL", "ClawR");
                break;
            case (int)Claw.Claw_Drift:
                TriggerClawAnimation(_angle, "ClawL_Drift", "ClawR_Drift");
                break;
        }
    }

    private void TriggerClawAnimation(float angle, string left, string right)
    {
        if(angle > 0)
        {
            _animator.SetTrigger(left);
        }
        else
        {
            _animator.SetTrigger(right);
        }
    }
}
