using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AndriusWalk : IPattern
{
    private Andrius _andrius;
    private Animator _animator;
    private NavMeshAgent _agent;
    private Transform _player;

    private float _distance;

    public void InitializePattern(Andrius andrius)
    {
        if(_andrius == null)
        {
            _andrius = andrius;
            _animator = _andrius.GetComponent<Animator>();
            _agent = _andrius.GetComponent<NavMeshAgent>();
        }

        _agent.enabled = true;
        _animator.applyRootMotion = false;
        _player = _andrius.PlayerTransform;
        _agent.SetDestination(_player.position);
    }
    public void UpdatePattern()
    {
        _distance = Distance();

        if (_distance > _agent.stoppingDistance)
        {
            _agent.SetDestination(_player.position);
        }
        else
        {
            _andrius.State.ChangeState(BossState.Attack);
        }
    }

    public void ExitPattern()
    {
        _agent.SetDestination(_andrius.transform.position);
        _agent.enabled = false;
        _animator.applyRootMotion = true;
    }

    private float Distance()
    {
        return Vector3.Distance(_player.position, _andrius.transform.position);
    }
}
