using System.Collections.Generic;
using UnityEngine;

public class FireHilichurlView : Enemy
{
    //Node
    private Node _node;

    //Patrol
    private List<Transform> _wayPointList;
    private List<Transform> _useWayPointList;
    private Transform _currentTransform;
    private float _waitTime = 4f;
    private float _waitCount = 0f;
    private bool iswaiting = true;

    //CheckPlayer
    private GameObject _player;
    private float _radius = 6f;
    private int _playerLayer;

    //Trace
    private bool isTrace = false;

    protected override void Awake()
    {
        base.Awake();
        InitializeFireHilichurl();
        InitializeWayPoint();

        _node = new Node(SetUpTree());
    }

    private void InitializeFireHilichurl()
    {
        enemyData = new EnemyData(200f, 200f, 3f, 0.1f, 180, Element.Fire);
        EnemyHealthDic.Add(this, enemyData.Health);
        HpSlider.maxValue = enemyData.Health;
        HpSlider.value = enemyData.Health;
        _playerLayer = LayerMask.GetMask("Player");
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
        attackNodeList.Add(new EnemyAction(AttackRange));
        attackNodeList.Add(new EnemyAction(AttackToPlayer));

        var attackSequence = new EnemySequence(attackNodeList);

        var checkPlayerNodeList = new List<INode>();
        checkPlayerNodeList.Add(new EnemyAction(CheckPlayer));
        checkPlayerNodeList.Add(new EnemyAction(IsTracking));
        checkPlayerNodeList.Add(new EnemyAction(TraceToPlayer));

        var checkSequence = new EnemySequence(checkPlayerNodeList);

        List<INode> selectorNodeList = new List<INode>();
        selectorNodeList.Add(attackSequence);
        selectorNodeList.Add(checkSequence);
        selectorNodeList.Add(new EnemyAction(Patrol));

        var selectorNode = new EnemySelector(selectorNodeList);

        return selectorNode;
    }

    private INode.NodeState Patrol()
    {
        if(_wayPointList == null || _wayPointList.Count == 0)
        {
            return INode.NodeState.Fail;
        }

        if (iswaiting)
        {
            _waitCount += Time.deltaTime;

            if(_waitCount >= _waitTime)
            {
                iswaiting = false;
                SetDestination(FindWayPoint().position, 3f, 0f, true);
            }
        }
        else
        {
            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                _waitCount = 0f;
                iswaiting = true;
                animator.SetBool("Move", false);
                return INode.NodeState.Success;
            }
        }

        return INode.NodeState.Running;
    }

    private INode.NodeState CheckPlayer()
    {
        if (isTrace)
            return INode.NodeState.Success;

        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius, _playerLayer);

        if(colliders.Length > 0)
        {
            isTrace = true;
            _player = colliders[0].gameObject;
            return INode.NodeState.Success;
        }

        return INode.NodeState.Fail;
    }

    private INode.NodeState TraceToPlayer()
    {
        if (_player == null)
            return INode.NodeState.Fail;

        SetDestination(_player.transform.position, 5f, 2f, false);

        return INode.NodeState.Running;
    }

    private INode.NodeState IsTracking()
    {
        if (!isTrace)
            return INode.NodeState.Fail;

        var animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

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

        isTrace = false;
        SetDestination(FindWayPoint().position, 3f, 0f, true);
        return INode.NodeState.Fail;
    }

    private INode.NodeState AttackRange()
    {
        bool isAttack = _player != null && Vector3.Distance(transform.position, _player.transform.position) <= 2f
            && isTrace;

        if (isAttack)
        {
            animator.SetBool("Trace", false);
            agent.isStopped = true;
            return INode.NodeState.Success;
        }
        else
        {
            agent.isStopped = false;
            return INode.NodeState.Fail;
        }
            
    }

    private INode.NodeState AttackToPlayer()
    {
        if(_player == null)
            return INode.NodeState.Fail;

        animator.SetTrigger("Attack");

        return INode.NodeState.Running;
    }

    private Transform FindWayPoint()
    {
        if (_useWayPointList == null || _useWayPointList.Count == 0)
        {
            _useWayPointList = new List<Transform>(_wayPointList);

            if(_currentTransform != null)
            {
                _useWayPointList.Remove(_currentTransform);
            }
        }

        Transform newRandomTransform = _useWayPointList[Random.Range(0,_useWayPointList.Count)];

        _useWayPointList.Remove(newRandomTransform);

        _currentTransform = newRandomTransform;

        return newRandomTransform;
    }

    private void SetDestination(Vector3 position, float speed, float stoppingDistance, bool isMoving)
    {
        animator.SetBool("Move", isMoving);
        animator.SetBool("Trace", !isMoving);
        agent.stoppingDistance = stoppingDistance;
        agent.speed = speed;
        agent.SetDestination(position);
    }
}
