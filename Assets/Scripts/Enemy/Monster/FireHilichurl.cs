using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireHilichurl : Enemy, IColor
{
    protected override void Awake()
    {
        base.Awake();
        InitState();
        
    }

    private void Start()
    {
        InitEnemyData();
    }

    private void InitState()
    {
        state = gameObject.AddComponent<EnemyStateMachine>();
        state.AddState(EnemyState.Idle, new FireHilichurlIdle(this));
        state.AddState(EnemyState.Move, new FireHilichurlMove(this));
        state.AddState(EnemyState.TraceAttack, new FireHilichurlTraceAttack(this));
        state.AddState(EnemyState.TraceMove, new FireHilichurlTraceMove(this));
    }

    private void InitEnemyData()
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
        _baseData = EnemyCSVLoader.Instance.GetEnemyCSVData<EnemyBaseData>("B101");
        _elementData = EnemyCSVLoader.Instance.GetEnemyCSVData<EnemyElementData>("E101");
        _traceData = EnemyCSVLoader.Instance.GetEnemyCSVData<EnemyTraceData>("T101");
        _overlapData = EnemyCSVLoader.Instance.GetEnemyCSVData<EnemyOverlapData>("O101");
    }

    public EnemyTraceData Data { get { return _traceData; } }
    public EnemyStateMachine State => state;
    public Animator Animator => animator;
    public NavMeshAgent Agent => agent;

    private Color color;
    public bool TraceAttack
    {
        get { return  attack; }
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

    //AnimationEvent------------------------------------------------------------------------
    public void OnAnimationEnd() 
    {
        if (Distance() > Agent.stoppingDistance)
            State.ChangeState(EnemyState.TraceMove);

        attack = true;
    }

    public void AttackOverlapBox()
    {
        Vector3 transformDirection = _overlapData.BoxList[0];

        Vector3 boxPosition = transform.position + transform.TransformDirection(transformDirection) + transform.forward;

        Vector3 boxSize = _overlapData.BoxList[1];

        Collider[] colliders = Physics.OverlapBox(boxPosition, boxSize /2, transform.rotation, LayerMask.GetMask("Player"));
        
        if(colliders.Length > 0)
        {
            Character player = colliders[0].transform.GetComponentInChildren<Character>();

            if(player != null)
            {
                player.TakeDamage(_baseData.Power);
            }
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 boxPosition = transform.position + transform.TransformDirection(new Vector3(0f, 0.8f, 0f)) + transform.forward;
        Gizmos.DrawWireCube(boxPosition, new Vector3(1f, 1, 0.5f));
    }

}

public abstract class FireHilichurlState : BaseState
{
    protected FireHilichurl fireHilichurl;

    public FireHilichurlState(FireHilichurl fireHilichurl)
    {
        this.fireHilichurl = fireHilichurl;
    }
}

public class FireHilichurlIdle : FireHilichurlState //기본 상태
{
    public FireHilichurlIdle(FireHilichurl fireHilichurl) : base(fireHilichurl) { }

    private float timer = 0f;

    public override void StateEnter()
    {
        fireHilichurl.MoveAnimation(0f);
    }

    public override void StateExit()
    {
        timer = 0f;
    }

    public override void StateUpDate()
    {
        fireHilichurl.Trace();

        timer += Time.deltaTime;

        if (timer > fireHilichurl.Data.NextMoveTime)
        {
            fireHilichurl.State.ChangeState(EnemyState.Move);
        }
    }
    
}

public class FireHilichurlMove : FireHilichurlState //이동 (배회)
{
    public FireHilichurlMove(FireHilichurl fireHilichurl) : base(fireHilichurl) { }

    List<Transform> WayPoint = new List<Transform>();
   
    public override void StateEnter()
    {
        FindMovePosition();
    }

    public override void StateExit()
    {
        fireHilichurl.SetDestination_This();
    }

    public override void StateUpDate()
    {
        fireHilichurl.Trace();

        if (fireHilichurl.Agent.remainingDistance <= fireHilichurl.Agent.stoppingDistance)
        {
            fireHilichurl.State.ChangeState(EnemyState.Idle);
        }
    }

    private void FindMovePosition()
    {
        GameObject movePoint = fireHilichurl.transform.parent.gameObject; //WayPoint Transform

        foreach (Transform point in movePoint.transform)
        {
            WayPoint.Add(point);
        }
        fireHilichurl.Agent.SetDestination(WayPoint[Random.Range(0, WayPoint.Count)].transform.position);

        fireHilichurl.MoveAnimation(3f);
    }
}

public class FireHilichurlTraceMove : FireHilichurlState //(추적 : 이동)
{
    public FireHilichurlTraceMove(FireHilichurl fireHilichurl) : base(fireHilichurl) { }
    
    public override void StateEnter()
    {
        fireHilichurl.SetDestination_Player();
        fireHilichurl.MoveAnimation(4f);
    }

    public override void StateExit()
    {
        fireHilichurl.SetDestination_This();
        fireHilichurl.MoveAnimation(0f);
    }

    public override void StateUpDate()
    {
        if (fireHilichurl.Distance() > fireHilichurl.Agent.stoppingDistance)
        {
            fireHilichurl.SetDestination_Player();
        }
        else if(fireHilichurl.Distance() <= fireHilichurl.Agent.stoppingDistance)
        {
            fireHilichurl.State.ChangeState(EnemyState.TraceAttack);
        }

        StopTracking();
    }

    private void StopTracking()
    {
        if (fireHilichurl.Distance() > fireHilichurl.Data.StopDistance)
        {
            fireHilichurl.State.ChangeState(EnemyState.Move);
        }
    }
}

public class FireHilichurlTraceAttack : FireHilichurlState //(추적 : 공격)
{
    public FireHilichurlTraceAttack(FireHilichurl fireHilichurl) : base(fireHilichurl) { }
    
    public override void StateEnter()
    {
        fireHilichurl.Agent.updateRotation = false;

        fireHilichurl.SetDestination_This();

        //fireHilichurl.MonsterWeapon.SetAttackPower(fireHilichurl.EnemyData.AttackPower);
    }

    public override void StateExit()
    {
        fireHilichurl.Agent.updateRotation = true;

        fireHilichurl.TraceAttack = true;
    }

    public override void StateUpDate()
    {
        if (fireHilichurl.TraceAttack)
        {
            fireHilichurl.TraceAttack = false;
            fireHilichurl.Animator.SetTrigger("Attack");
        }

        fireHilichurl.TraceAttackRotation();
    }
}