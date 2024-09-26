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
        Pattern();
    }

    private void Back()
    {
        _animator.SetTrigger(_back);
    }

}
