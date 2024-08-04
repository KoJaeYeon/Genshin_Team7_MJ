using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampAttack : IPattern
{
    private Andrius _andrius;
    private Animator _animator;

    private Vector3 _targetPos;

    private float Rotspeed = 15.0f;

    public void InitializePattern(Andrius andrius)
    {
        if(_andrius == null)
        {
            _andrius = andrius;
            _animator = _andrius.GetComponent<Animator>();
        }

        _targetPos = _andrius.PlayerTransform.position;
        _animator.SetTrigger("Stamp");
    }

    public void UpdatePattern()
    {
        var animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if(animatorStateInfo.IsName("Stamp") && animatorStateInfo.normalizedTime < 0.4f)
        {
            RotateToPlayer();
        }
    }

    public void ExitPattern() { }
    
    private void RotateToPlayer()
    {
        Vector3 targetDirection = _targetPos - _andrius.transform.position;

        targetDirection.y = 0f;

        float rotateAngle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(0f, rotateAngle, 0f);

        _andrius.transform.rotation = Quaternion.Slerp(_andrius.transform.rotation, rotation, Rotspeed * Time.deltaTime);
    }   
}
