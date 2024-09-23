using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LightningHilichurl : Enemy, IColor
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
        state.AddState(EnemyState.Idle, new LightningHilichurlIdle(this));
        state.AddState(EnemyState.Move, new LightningHilichurlMove(this));
        state.AddState(EnemyState.TraceMove, new LightningHilichurlTraceMove(this));
        state.AddState(EnemyState.TraceAttack, new LightningHilichurlTraceAttack(this));
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
        _baseData = EnemyCSVLoader.Instance.GetEnemyCSVData<EnemyBaseData>("B104");
        _elementData = EnemyCSVLoader.Instance.GetEnemyCSVData<EnemyElementData>("E104");
        _traceData = EnemyCSVLoader.Instance.GetEnemyCSVData<EnemyTraceData>("T104");
        _overlapData = EnemyCSVLoader.Instance.GetEnemyCSVData<EnemyOverlapData>("O104");
    }

    public EnemyTraceData TraceData { get { return _traceData; } }
    private Color color;
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

    //AnimationEvent---------------------------------------------
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

public abstract class LightningHilichurlState : BaseState
{
    protected LightningHilichurl lightningHilichurl;

    public LightningHilichurlState(LightningHilichurl lightningHilichurl)
    {
        this.lightningHilichurl = lightningHilichurl;
    }
}

public class LightningHilichurlIdle : LightningHilichurlState //기본 상태
{
    public LightningHilichurlIdle(LightningHilichurl lightningHilichurl) : base(lightningHilichurl) { }

    private float timer = 0f;

    public override void StateEnter()
    {
        lightningHilichurl.MoveAnimation(0f);
    }

    public override void StateExit()
    {
        timer = 0f;
    }

    public override void StateUpDate()
    {
        lightningHilichurl.Trace();
       
        timer += Time.deltaTime;

        if (timer > lightningHilichurl.TraceData.NextMoveTime)
        {
            lightningHilichurl.State.ChangeState(EnemyState.Move);
        }
    }
}

public class LightningHilichurlMove : LightningHilichurlState //이동 (배회)
{
    public LightningHilichurlMove(LightningHilichurl lightningHilichurl) : base(lightningHilichurl) { }

    List<Transform> WayPoint = new List<Transform>();

    public override void StateEnter()
    {
        FindMovePosition();
    }

    public override void StateExit()
    {
        lightningHilichurl.SetDestination_This();
    }

    public override void StateUpDate()
    {
        lightningHilichurl.Trace();

        if (lightningHilichurl.Agent.remainingDistance <= lightningHilichurl.Agent.stoppingDistance)
        {
            lightningHilichurl.State.ChangeState(EnemyState.Idle);
        }
    }

    private void FindMovePosition()
    {
        GameObject movePoint = lightningHilichurl.transform.parent.gameObject;

        foreach (Transform point in movePoint.transform)
        {
            WayPoint.Add(point);
        }

        lightningHilichurl.Agent.SetDestination(WayPoint[Random.Range(0, WayPoint.Count)].transform.position);

        lightningHilichurl.MoveAnimation(3f);

    }
}

public class LightningHilichurlTraceMove : LightningHilichurlState //이동 (추적)
{
    public LightningHilichurlTraceMove(LightningHilichurl lightningHilichurl) : base(lightningHilichurl) { }

    public override void StateEnter()
    {
        lightningHilichurl.SetDestination_Player();
        lightningHilichurl.MoveAnimation(4f);
    }

    public override void StateExit()
    {
        lightningHilichurl.SetDestination_This();
        lightningHilichurl.MoveAnimation(0f);
    }

    public override void StateUpDate()
    {
        if (lightningHilichurl.Distance() > lightningHilichurl.Agent.stoppingDistance)
        {
            lightningHilichurl.SetDestination_Player();
        }
        else if (lightningHilichurl.Distance() <= lightningHilichurl.Agent.stoppingDistance)
        {
            lightningHilichurl.State.ChangeState(EnemyState.TraceAttack);
        }

        StopTracking();
    }

    private void StopTracking()
    {
        if (lightningHilichurl.Distance() > lightningHilichurl.TraceData.StopDistance)
        {
            lightningHilichurl.State.ChangeState(EnemyState.Move);
        }
    }

}

public class LightningHilichurlTraceAttack : LightningHilichurlState
{
    public LightningHilichurlTraceAttack(LightningHilichurl lightningHilichurl) : base(lightningHilichurl) { }

    public override void StateEnter()
    {
        lightningHilichurl.Agent.updateRotation = false;

        lightningHilichurl.SetDestination_This();
    }

    public override void StateExit()
    {
        lightningHilichurl.Agent.updateRotation = true;
        lightningHilichurl.TraceAttack = true;
    }

    public override void StateUpDate()
    {
        if (lightningHilichurl.TraceAttack)
        {
            lightningHilichurl.TraceAttack = false;
            lightningHilichurl.Animator.SetTrigger("Attack");
        }

        lightningHilichurl.TraceAttackRotation();
    }
}