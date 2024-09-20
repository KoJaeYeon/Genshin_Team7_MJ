using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class IceHilichurl : Enemy, IColor
{
    protected override void Awake()
    {
        base.Awake();
        InitState();
        
    }

    private void Start()
    {
        InitData();
    }

    private void InitState()
    {
        state = gameObject.AddComponent<EnemyStateMachine>();
        state.AddState(EnemyState.Idle, new IceHilichurlIdle(this));
        state.AddState(EnemyState.Move, new IceHilichurlMove(this));
        state.AddState(EnemyState.TraceMove, new IceHilichurlTraceMove(this));
        State.AddState(EnemyState.TraceAttack, new IceHilichurlTraceAttack(this));
    }

    private void InitData()
    {
        GetData();

        EnemyHealthDic.Add(this, _baseData.Health);
        HpSlider.maxValue = _baseData.Health;
        HpSlider.value = _baseData.Health;
        agent.speed = _baseData.Speed;
        agent.stoppingDistance = _traceData.AgentStopDistance;
        traceDistance = _traceData.TraceDistance;
        color = _elementData.Color;
    }

    private void GetData()
    {
        _baseData = EnemyCSVLoder.Instance.GetEnemyCSVData<EnemyBaseData>("B102");
        _elementData = EnemyCSVLoder.Instance.GetEnemyCSVData<EnemyElementData>("E102");
        _traceData = EnemyCSVLoder.Instance.GetEnemyCSVData<EnemyTraceData>("T102");
        _overlapData = EnemyCSVLoder.Instance.GetEnemyCSVData<EnemyOverlapData>("O102");
    }

    
    private Color color;
    public EnemyTraceData TraceData { get { return _traceData; } }
    public EnemyStateMachine State => state;
    public Animator Animator => animator;
    public NavMeshAgent Agent => agent;
    public bool TraceAttack
    {
        get { return attack; }
        set { attack = value; }
    }

    public Color GetColor()
    {
        return color;
    }

    public override void Splash(float damage)
    {
        EnemyHealthDic[this] -= damage;

        HpSlider.value = EnemyHealthDic[this];

        if (EnemyHealthDic[this] <= 0)
        {
            StartCoroutine(Die(this, null));
        }
    }

    //AnimationEvent-------------------------------------
    public void OnAnimationEnd()
    {
        if (Distance() > Agent.stoppingDistance)
        {
            State.ChangeState(EnemyState.TraceMove);
        }

        attack = true;
    }

    public void AttackOverlapBox()
    {
        Vector3 transformDirection = _overlapData.BoxList[0];

        Vector3 boxPosition = transform.position + transform.TransformDirection(transformDirection) + transform.forward;

        Vector3 boxSize = _overlapData.BoxList[1];

        Collider[] colliders = Physics.OverlapBox(boxPosition, boxSize /2, transform.rotation, LayerMask.GetMask("Player"));

        if (colliders.Length > 0)
        {
            Character player = colliders[0].transform.GetComponentInChildren<Character>();

            if (player != null)
            {
                player.TakeDamage(_baseData.Power);
            }
        }
    }
}

public abstract class IceHilichurlState : BaseState
{
    protected IceHilichurl iceHilichurl;

    public IceHilichurlState(IceHilichurl iceHilichurl)
    {
        this.iceHilichurl = iceHilichurl;
    }
}

public class IceHilichurlIdle : IceHilichurlState
{
    public IceHilichurlIdle(IceHilichurl iceHilichurl) : base(iceHilichurl) { }

    private float timer = 0f;

    public override void StateEnter()
    {
        iceHilichurl.MoveAnimation(0f);
    }

    public override void StateExit()
    {
        timer = 0f;
    }

    public override void StateUpDate()
    {
        iceHilichurl.Trace();

        timer += Time.deltaTime;

        if (timer > iceHilichurl.TraceData.NextMoveTime)
        {
            iceHilichurl.State.ChangeState(EnemyState.Move);
        }
    }
}

public class IceHilichurlMove : IceHilichurlState
{
    public IceHilichurlMove(IceHilichurl iceHilichurl) : base(iceHilichurl) { }

    List<Transform> WayPoint = new List<Transform>();
   
    public override void StateEnter()
    {
        FindMovePosition();
    }

    public override void StateExit()
    {
        iceHilichurl.Agent.SetDestination(iceHilichurl.Agent.transform.position);
    }

    public override void StateUpDate()
    {
        iceHilichurl.Trace();

        if (iceHilichurl.Agent.remainingDistance <= iceHilichurl.Agent.stoppingDistance)
        {
            iceHilichurl.State.ChangeState(EnemyState.Idle);
        }
    }
    private void FindMovePosition()
    {
        GameObject movePoint = iceHilichurl.transform.parent.gameObject;

        foreach (Transform point in movePoint.transform)
        {
            WayPoint.Add(point);
        }

        iceHilichurl.Agent.SetDestination(WayPoint[Random.Range(0, WayPoint.Count)].transform.position);
        iceHilichurl.MoveAnimation(3f);
    }
}

public class IceHilichurlTraceMove : IceHilichurlState
{
    public IceHilichurlTraceMove(IceHilichurl iceHilichurl) : base(iceHilichurl) { }
 
    public override void StateEnter()
    {
        iceHilichurl.SetDestination_Player();
        iceHilichurl.MoveAnimation(4f);
    }

    public override void StateExit()
    {
        iceHilichurl.SetDestination_This();
        iceHilichurl.MoveAnimation(0f);
    }

    public override void StateUpDate()
    {
        if (iceHilichurl.Distance() > iceHilichurl.Agent.stoppingDistance)
        {
            iceHilichurl.SetDestination_Player();
        }
        else if (iceHilichurl.Distance() <= iceHilichurl.Agent.stoppingDistance)
        {
            iceHilichurl.State.ChangeState(EnemyState.TraceAttack);
        }

        StopTracking();
    }

    private void StopTracking()
    {
        if (iceHilichurl.Distance() > iceHilichurl.TraceData.StopDistance)
        {
            iceHilichurl.State.ChangeState(EnemyState.Move);
        }
    }
}

public class IceHilichurlTraceAttack : IceHilichurlState
{
    public IceHilichurlTraceAttack(IceHilichurl iceHilichurl) : base(iceHilichurl) { }
    
    public override void StateEnter()
    {
        iceHilichurl.Agent.updateRotation = false;

        iceHilichurl.SetDestination_This();

        //iceHilichurl.MonsterWeapon.SetAttackPower(iceHilichurl.Data.Power);   
    }

    public override void StateExit()
    {
        iceHilichurl.Agent.updateRotation = true;

        iceHilichurl.TraceAttack = true;
    }

    public override void StateUpDate()
    {
        if (iceHilichurl.TraceAttack)
        {
            iceHilichurl.Animator.SetTrigger("Attack");
            iceHilichurl.TraceAttack = false;
        }

        iceHilichurl.TraceAttackRotation();
    }
}