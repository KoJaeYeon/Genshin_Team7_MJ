using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Andrius_Turn : Andrius_AttackController
{
    public Andrius_Turn(Andrius andrius) : base(andrius) { }

    public override void StateEnter()
    {
        _andrius.IsAction = true;

        Turn();
    }

    public override void StateFixedUpdate()
    {
        NextPattern();
    }

    private void Turn()
    {
        if(CalculateSignedAngle() > 0)
        {
            _animator.SetTrigger(_turnRight);
        }
        else
        {
            _animator.SetTrigger(_turnLeft);
        }
    }
}
