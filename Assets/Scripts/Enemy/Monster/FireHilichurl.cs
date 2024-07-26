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
        //체력 , 공격력, 이동속도, 물리내성, 경험치 , 속성
        enemyData = new EnemyData(200f, 200f, 3f, 0.1f, 180, Element.Fire);
        EnemyHealthDic.Add(this, enemyData.Health);
        HpSlider.maxValue = enemyData.Health;
        HpSlider.value = enemyData.Health;
    }

    public EnemyStateMachine State => state;
    public Animator Animator => animator;
    public NavMeshAgent Agent => agent;
    public MonsterWeapon MonsterWeapon => Weapon;
    public EnemyData EnemyData => enemyData;

    private Color color = Color.red;
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

        if (timer > 4.0f)
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
        if (fireHilichurl.Distance() > 15.0f)
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

        fireHilichurl.MonsterWeapon.SetAttackPower(fireHilichurl.EnemyData.AttackPower);
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
            fireHilichurl.MonsterWeapon.EableSword();
        }

        fireHilichurl.TraceAttackRotation();
    }
}