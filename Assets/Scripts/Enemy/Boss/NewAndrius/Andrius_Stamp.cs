using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Andrius_Stamp : Andrius_AttackController
{
    public Andrius_Stamp(Andrius andrius) : base(andrius) { }

    private Vector3 _targetPosition;

    public override void StateEnter()
    {
        bool paralyzation = Paralyzation();

        if (paralyzation)
        {
            return;
        }

        _andrius.IsAction = true;

        _targetPosition = _playerTransform.position;

        _animator.SetTrigger(_stamp);
    }

    public override void StateFixedUpdate()
    {
        var animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (animatorStateInfo.IsName("Stamp"))
        {
            if(animatorStateInfo.normalizedTime >= 0.2f && animatorStateInfo.normalizedTime <= 0.5f)
            {
                RotateToPlayer();
            }
        }

        NextPattern();
    }

    private void RotateToPlayer()
    {
        Vector3 targetDirection = _targetPosition - _andrius.transform.position;

        targetDirection.y = 0f;

        float rotateAngle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(0f, rotateAngle, 0f);

        _andrius.transform.rotation = Quaternion.Slerp(_andrius.transform.rotation, rotation, _stampData.RotationSpeed * Time.deltaTime);
    }
}