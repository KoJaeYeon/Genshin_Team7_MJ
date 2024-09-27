using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Andrius_Howl : Andrius_AttackController
{
    public Andrius_Howl(Andrius andrius) : base(andrius) { }

    private enum SelectHowl
    {
        howl,
        iceRain
    }

    public override void StateEnter()
    {
        if (_andrius.Paralyzation <= 0)
        {
            _state.ChangeState(BossState.Idle);

            return;
        }

        _andrius.IsAction = true;

        RandomHowl();
    }

    public override void StateFixedUpdate()
    {
        NextPattern();
    }

    public void RandomHowl()
    {
        int random = Random.Range(0, 2);

        switch (random)
        {
            case (int)SelectHowl.howl:
                _animator.SetTrigger(_howl);
                break;
            case (int)SelectHowl.iceRain:
                _animator.SetTrigger(_iceRain);
                break;
        }
    }
}
