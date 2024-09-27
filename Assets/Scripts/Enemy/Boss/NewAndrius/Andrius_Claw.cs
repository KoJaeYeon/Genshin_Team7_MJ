using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Andrius_Claw : Andrius_AttackController
{
    public Andrius_Claw(Andrius andrius) : base(andrius) { }

    private enum Claw
    {
        Claw,
        Claw_Drift
    }

    private bool _isHit = false;

    public override void StateEnter()
    {
        if(_andrius.Paralyzation <= 0)
        {
            _state.ChangeState(BossState.Idle);

            return; 
        }

        _andrius.IsAction = true;

        ClawAttack();
    }

    public override void StateFixedUpdate()
    {
        NextPattern();
    }

    public override void StateExit()
    {
        _isHit = false;
    }

    private void ClawAttack()
    {
        float angle = CalculateSignedAngle();

        int random = Random.Range(0, 2);

        switch(random)
        {
            case (int)Claw.Claw:
                TriggerClawAnimation(angle, _clawLeft, _clawRigt);
                break;
            case (int)Claw.Claw_Drift:
                TriggerClawAnimation(angle, _claw_LeftDrift, _claw_RightDrift);
                break;
        }
    }

    private void TriggerClawAnimation(float angle, int left, int right)
    {
        if (angle > 0)
        {
            _animator.SetTrigger(left);
        }
        else
        {
            _animator.SetTrigger(right);
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !_isHit)
        {
            _isHit = true;

            Character player = other.gameObject.GetComponentInChildren<Character>();

            if (player != null)
            {
                player.TakeDamage(_andrius.GetAtk() * _clawData.SkillDamage);
            }
        }
    }
}
