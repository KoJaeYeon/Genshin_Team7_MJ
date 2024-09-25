using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Andrius_Drift : Andrius_AttackController
{
    public Andrius_Drift(Andrius andrius) : base(andrius) { }

    public override void StateEnter()
    {
        base.StateEnter();

        _andrius.IsAction = true;

        DriftAttack();
    }

    public override void StateFixedUpdate()
    {
        NextPattern();
    }

    private void DriftAttack()
    {
        float angle = CalculateSignedAngle();

        if(angle < 0)
        {
            _animator.SetTrigger(_drift_Right);
        }
        else
        {
            _animator.SetTrigger(_drift_Left);
        }

    }
}

