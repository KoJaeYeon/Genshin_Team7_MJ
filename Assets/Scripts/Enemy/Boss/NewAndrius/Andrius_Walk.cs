using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Andrius_Walk : Andrius_AttackController
{
    private WaitForSeconds _walkTime;
    private List<Transform> _walkList;

    private int _walkCount;
    private float _currentSpeed;
    private bool _isFirstWalking;

    public Andrius_Walk(Andrius andrius) : base(andrius)
    {
        _walkTime = new WaitForSeconds(_walkData.WalkTime);

        _walkList = new List<Transform>(_andrius.WalkList);

        _isFirstWalking = true;

        _walkCount = 0;
    }

    public override void StateEnter()
    {
        _rig.weight = 1f;

        _agent.enabled = true;

        _animator.applyRootMotion = false;

        OnEnterWalk();
    }
    public override void StateFixedUpdate()
    {
        OnUpdateWalk();
    }

    public override void StateExit()
    {
        _rig.weight = 0;

        _agent.speed = _currentSpeed;

        _agent.SetDestination(_andrius.transform.position);

        _agent.enabled = false;

        _animator.applyRootMotion = true;
    }

    private void OnEnterWalk()
    {
        if (!_isFirstWalking)
        {
            _agent.SetDestination(_playerTransform.position);

            return;
        }
        
        _currentSpeed = _agent.speed;

        _agent.speed = _walkData.Speed;

        _agent.SetDestination(_walkList[_walkCount].position);

        _andrius.StartCoroutine(WalkCoroutine());
    }

    private void OnUpdateWalk()
    {
        if (_isFirstWalking)
        {
            if(_agent.remainingDistance <= _agent.stoppingDistance)
            {
                _walkCount = (_walkCount + 1) % _walkList.Count;

                _agent.SetDestination(_walkList[_walkCount].position);
            }
        }
        else
        {
            float distance = CalculateDistance();

            if (distance > _agent.stoppingDistance)
            {
                _agent.SetDestination(_playerTransform.position);
            }
            else
            {
                NextPattern();
            }
        }
    }
    
    private IEnumerator WalkCoroutine()
    {
        yield return _walkTime;

        _isFirstWalking = false;

        NextPattern();
    }
}
