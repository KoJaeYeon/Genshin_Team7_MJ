using System.Collections.Generic;
using UnityEngine;

public class FireHilichurlView : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        InitializeFireHilichurl();
        InitializeWayPoint();
    }

    private void InitializeFireHilichurl()
    {
        _data = new EnemyData(200f, 200f, 3f, 0.1f, 180, Element.Fire);
        _enemyHealthDic.Add(this, _data.Health);
        _hpSlider.maxValue = _data.Health;
        _hpSlider.value = _data.Health;
        _node = new Node(SetUpTree());
    }

    private void InitializeWayPoint()
    {
        _wayPointList = new List<Transform>();

        GameObject wayPoint = transform.parent.gameObject;

        foreach(Transform point in wayPoint.transform)
        {
            _wayPointList.Add(point);
        }

        _wayPointList.RemoveAt(_wayPointList.Count - 1);

        _useWayPointList = new List<Transform>(_wayPointList);
    }

    private void Update()
    {
        _node.Execute();
    }

    private INode SetUpTree()
    {
        var attackNodeList = new List<INode>();
        attackNodeList.Add(new EnemyAction(CheckAttackRange));
        attackNodeList.Add(new EnemyAction(EnemyAttack));

        var attackSequence = new EnemySequence(attackNodeList);

        var checkPlayerNodeList = new List<INode>();
        checkPlayerNodeList.Add(new EnemyAction(CheckPlayer));
        checkPlayerNodeList.Add(new EnemyAction(IsTracking));
        checkPlayerNodeList.Add(new EnemyAction(TrackingPlayer));

        var checkSequence = new EnemySequence(checkPlayerNodeList);

        List<INode> selectorNodeList = new List<INode>();
        selectorNodeList.Add(attackSequence);
        selectorNodeList.Add(checkSequence);
        selectorNodeList.Add(new EnemyAction(Patrol));

        var selectorNode = new EnemySelector(selectorNodeList);

        return selectorNode;
    }

    public override INode.NodeState Patrol()
    {
        if(_wayPointList == null || _wayPointList.Count == 0)
        {
            return INode.NodeState.Fail;
        }

        if (isWaiting)
        {
            _waitCount += Time.deltaTime;

            if(_waitCount >= _waitTime)
            {
                isWaiting = false;
                SetDestination(FindWayPoint(_useWayPointList, _wayPointList, ref _currentTransform).position, 3f, 0f, true);
            }
        }
        else
        {
            if(_agent.remainingDistance <= _agent.stoppingDistance)
            {
                _waitCount = 0f;
                isWaiting = true;
                _animator.SetBool("Move", false);
                return INode.NodeState.Success;
            }
        }

        return INode.NodeState.Running;
    }

    public override INode.NodeState CheckPlayer()
    {
        if (isTracking)
            return INode.NodeState.Success;

        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius, _playerLayer);

        if(colliders.Length > 0)
        {
            isTracking = true;
            _player = colliders[0].gameObject;
            return INode.NodeState.Success;
        }

        return INode.NodeState.Fail;
    }

    public override INode.NodeState TrackingPlayer()
    {
        if (_player == null)
            return INode.NodeState.Fail;

        SetDestination(_player.transform.position, 5f, 2f, false);

        return INode.NodeState.Running;
    }

    public override INode.NodeState IsTracking()
    {
        if (!isTracking)
            return INode.NodeState.Fail;

        var animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (animatorStateInfo.IsName("atk_02") && animatorStateInfo.normalizedTime < 1.0f)
        {
            return INode.NodeState.Running;
        }

        bool traceToPlayer = _player != null
                && Vector3.Distance(transform.position, _player.transform.position) < 15f;

        if (traceToPlayer)
        {
            SetDestination(_player.transform.position, 5f, 2f, false);
            return INode.NodeState.Success;
        }

        isTracking = false;
        SetDestination(FindWayPoint(_useWayPointList, _wayPointList, ref _currentTransform).position, 3f, 0f, true);
        return INode.NodeState.Fail;
    }

    public override INode.NodeState CheckAttackRange()
    {
        bool isAttack = _player != null && Vector3.Distance(transform.position, _player.transform.position) <= 2f
            && isTracking;

        if (isAttack)
        {
            _animator.SetBool("Trace", false);
            _agent.isStopped = true;
            return INode.NodeState.Success;
        }
        else
        {
            _agent.isStopped = false;
            return INode.NodeState.Fail;
        }
            
    }

    public override INode.NodeState EnemyAttack()
    {
        if(_player == null)
            return INode.NodeState.Fail;

        _animator.SetTrigger("Attack");

        return INode.NodeState.Running;
    }
}
