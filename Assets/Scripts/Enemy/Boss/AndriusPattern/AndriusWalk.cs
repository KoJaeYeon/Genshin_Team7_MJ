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
    private AndriusWalkData _walkData;

    public AndriusWalk()
    {
        _walkData = EnemyCSVLoder.Instance.GetAndriusCSVData<AndriusWalkData>(PatternName.AndriusWalk);
        Debug.Log($"AndriusWalk : {_walkData.Speed},{_walkData.WalkTime}");
    }

    private float _distance;

    private bool _isFirstWalking = true;
    private int _walkIndex = 0;
    private float _currentSpeed;

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

        if (!_isFirstWalking)
        {
            _agent.SetDestination(_player.position);
        }
        else
        {
            _currentSpeed = _agent.speed;

            _agent.speed = _walkData.Speed;

            _agent.SetDestination(_andrius.WalkList[_walkIndex].transform.position);

            _andrius.StartCoroutine(WalkCoroutine());
        }
    }
    public void UpdatePattern()
    {
        if (_isFirstWalking)
        {
            if(_agent.remainingDistance <= _agent.stoppingDistance)
            {
                _walkIndex = (_walkIndex + 1) % _andrius.WalkList.Count;

                _agent.SetDestination(_andrius.WalkList[_walkIndex].transform.position);
            }
        }
        else
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
    }

    private IEnumerator WalkCoroutine()
    {
        yield return new WaitForSeconds(_walkData.WalkTime);

        _isFirstWalking = false;

        _andrius.State.ChangeState(BossState.Attack);
    }

    public void ExitPattern()
    {
        _agent.speed = _currentSpeed;
        _agent.SetDestination(_andrius.transform.position);
        _agent.enabled = false;
        _animator.applyRootMotion = true;
    }

    private float Distance()
    {
        return Vector3.Distance(_player.position, _andrius.transform.position);
    }
}
