using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Andrius_Stamp : Andrius_AttackController
{
    public Andrius_Stamp(Andrius andrius) : base(andrius) { }

    private Vector3 _targetPosition;

    public override void StateEnter()
    {
        base.StateEnter();

        _andrius.IsAction = true;

        _targetPosition = _playerTransform.position;

        _animator.SetTrigger(_stamp);
    }

    public override void StateFixedUpdate()
    {
        var animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (animatorStateInfo.IsName("Stamp") && animatorStateInfo.normalizedTime < _stampData.MaxNormalizedTime)
        {
            RotateToPlayer();
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