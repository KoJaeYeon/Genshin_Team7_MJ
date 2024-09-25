using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Andrius_Idle : Andrius_AttackController
{
    private WaitForSeconds _changeTime;

    public Andrius_Idle(Andrius andrius) : base(andrius)
    {
        _changeTime = new WaitForSeconds(_paralyzationData.ChangeTime);
    }

    public override void StateEnter()
    {
        _rig.weight = 1f;

        _animator.SetBool(_idle, true);

        _andrius.StartCoroutine(Next());
    }

    public override void StateExit()
    {
        _rig.weight = 0f;

        _animator.SetBool(_idle, false);

        _andrius.Paralyzation = _paralyzationData.ParalyzationValue;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _animator.SetTrigger(_hit);
        }
    }

    private IEnumerator Next()
    {
        yield return _changeTime;

        NextPattern();
    }
}
