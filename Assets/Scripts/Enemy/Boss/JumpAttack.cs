using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttack : IPattern
{
    private Andrius _andrius;
    private Animator _animator;
    private Transform _player;
    private Rigidbody _rigidBody;

    private Vector3 _endPos;
    private float _moveSpeed = 12.0f;
    private float _rotationSpeed = 5f;
    
    public void InitializePattern(Andrius andrius)
    {
        if(_andrius == null)
        {
            _andrius = andrius;
            _animator = _andrius.GetComponent<Animator>();
            _rigidBody = _andrius.GetComponent<Rigidbody>();
        }

        _animator.SetTrigger("JumpAttack");
        _player = _andrius.PlayerTransform;
        _endPos = _player.position;
        _rigidBody.isKinematic = true;
    }

    public void UpdatePattern()
    {
        var animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if(animatorStateInfo.IsName("Jump") && animatorStateInfo.normalizedTime < 0.3f)
        {
            RotateToPlayer();
            MoveToPlayer();
        }
    }

    public void ExitPattern()
    {
        _rigidBody.isKinematic = false;
    }

    private void RotateToPlayer()
    {
        Vector3 targetDirection = _endPos - _andrius.transform.position;

        targetDirection.y = 0f;

        float angle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(0f, angle, 0f);

        _andrius.transform.rotation = Quaternion.Slerp(_andrius.transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
    }

    private void MoveToPlayer()
    {
        Vector3 targetDirection = (_endPos - _andrius.transform.position).normalized;

        Vector3 move = targetDirection * _moveSpeed * Time.fixedDeltaTime;

        if(Vector3.Distance(_andrius.transform.position, _endPos) > move.magnitude)
        {
            _andrius.transform.Translate(move, Space.World);
        }
        else
        {
            _andrius.transform.position = _endPos;
        }
    }
}
