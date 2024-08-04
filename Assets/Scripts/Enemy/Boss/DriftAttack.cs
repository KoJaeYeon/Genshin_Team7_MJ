using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftAttack : IPattern
{
    private Andrius _andrius;
    private Animator _animator;
    private Transform _player;

    private float _angle;

    public void InitializePattern(Andrius andrius)
    {
        if (_andrius == null)
        {
            _andrius = andrius;
            _animator = _andrius.GetComponent<Animator>();
        }

        _player = _andrius.PlayerTransform;
        Drift();
    }

    public void UpdatePattern() { }
    public void ExitPattern() { }
    
    private void Drift()
    {
        Vector3 targetDirection = (_player.position - _andrius.transform.position).normalized;

        Vector3 andriusForward = _andrius.transform.forward;

        _angle = Vector3.SignedAngle(andriusForward, targetDirection, Vector3.up);

        if(_angle < 0)
        {
            _animator.SetTrigger("DriftR");
        }
        else
        {
            _animator.SetTrigger("DriftL");
        }
        
    }
}
