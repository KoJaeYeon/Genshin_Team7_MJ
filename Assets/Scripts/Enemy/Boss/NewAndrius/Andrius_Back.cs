using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Andrius_Back : Andrius_AttackController
{
    public Andrius_Back(Andrius andrius) : base(andrius) { }

    public override void StateEnter()
    {
        _andrius.IsAction = true;

        Back();
    }

    public override void StateFixedUpdate()
    {
        NextPattern();
    }

    private void Back()
    {
        float signedAngle = CalculateSignedAngle();

        Rotation(signedAngle);

        _animator.SetTrigger(_back);
    }

    private void Rotation(float signedAngle)
    {
        _andrius.transform.rotation = Quaternion.Euler(0,signedAngle, 0);
    }

}
