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
        //체력 , 공격력, 이동속도, 물리내성, 경험치 , 속성
        enemyData = new EnemyData(230f, 150f, 2f, 0.1f, 130, Element.Ice);
        EnemyHealthDic.Add(this, enemyData.Health);

        HpSlider.maxValue = enemyData.Health;
        HpSlider.value = enemyData.Health;
    }

    private Color color = Color.blue;
    public EnemyStateMachine State => state;
    public Animator Animator => animator;
    public MonsterWeapon MonsterWeapon => Weapon;
    public NavMeshAgent Agent => agent;
    public EnemyData EnemyData => enemyData;
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

        if (timer > 4.0f)
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
        if (iceHilichurl.Distance() > 15.0f)
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

        iceHilichurl.MonsterWeapon.SetAttackPower(iceHilichurl.EnemyData.AttackPower);   
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
            iceHilichurl.MonsterWeapon.EableSword();
        }

        iceHilichurl.TraceAttackRotation();
    }
}