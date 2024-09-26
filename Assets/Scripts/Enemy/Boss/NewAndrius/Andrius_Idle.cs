using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Andrius_Idle : Andrius_AttackController
{
    private WaitForSeconds _changeTime;
    private float _startTime;
    private float _currentTime;

    public Andrius_Idle(Andrius andrius) : base(andrius)
    {
        _changeTime = new WaitForSeconds(_paralyzationData.ChangeTime);
    }

    public override void StateEnter()
    {
        _rig.weight = 1f;

        _animator.SetBool(_idle, true);

        _startTime = Time.time;
    }

    public override void StateFixedUpdate()
    {
        _currentTime = Time.time;

        if(_currentTime - _startTime > _paralyzationData.ChangeTime)
        {
            _andrius.Paralyzation = _paralyzationData.ParalyzationValue;

            NextPattern();
        }
    }

    public override void StateExit()
    {
        _rig.weight = 0f;

        _animator.SetBool(_idle, false);
    }

    private IEnumerator Next()
    {
        yield return _changeTime;

        _andrius.Paralyzation = _paralyzationData.ParalyzationValue;

        NextPattern();
    }
}
