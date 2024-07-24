using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NomalHilichurl : Enemy, IColor
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
        state.AddState(EnemyState.Idle, new NomalHilichurlIdle(this));
        state.AddState(EnemyState.Move, new NomalHilichurlMove(this));
        state.AddState(EnemyState.TraceMove, new NomalHilichurlTraceMove(this));
        state.AddState(EnemyState.TraceAttack, new NomalHilichurlTraceAttack(this));
    }
    private void InitData()
    {
        //체력 , 공격력, 이동속도, 물리내성, 경험치 , 속성
        enemyData = new EnemyData(220f, 100f, 3f, 0.1f, 100, Element.Normal);
        EnemyHealthDic.Add(this, enemyData.Health);

        HpSlider.maxValue = enemyData.Health;
        HpSlider.value = enemyData.Health;
    }

    public EnemyStateMachine State => state;
    public Animator Animator => animator;
    public MonsterWeapon MonsterWeapon => Weapon;
    public NavMeshAgent Agent => agent;
    public EnemyData EnemyData => enemyData;

    private Color color = Color.white;
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

    //AnimationEvent--------------------------------------------
    public void OnAnimationEnd()
    {
        if (Distance() > Agent.stoppingDistance)
        {
            state.ChangeState(EnemyState.TraceMove);
        }            

        attack = true;

    }
}

public abstract class NomalHilichurlState : BaseState
{
    protected NomalHilichurl nomalHilichurl;

    public NomalHilichurlState(NomalHilichurl nomalHilichurl)
    {
        this.nomalHilichurl = nomalHilichurl;
    }
}

public class NomalHilichurlIdle : NomalHilichurlState //기본 상태
{
    public NomalHilichurlIdle(NomalHilichurl nomalHilichurl) : base(nomalHilichurl) { }

    private float timer = 0f;    
   
    public override void StateEnter()
    {
        nomalHilichurl.MoveAnimation(0f);
    }

    public override void StateExit()
    {
        timer = 0f;
    }

    public override void StateUpDate()
    {
        nomalHilichurl.Trace();
        
        timer += Time.deltaTime;

        if(timer > 4.0f)
        {
            nomalHilichurl.State.ChangeState(EnemyState.Move);
        }
    }
}

public class NomalHilichurlMove : NomalHilichurlState //이동 (배회)
{
    public NomalHilichurlMove(NomalHilichurl nomalHilichurl) : base(nomalHilichurl) { }

    List<Transform> WayPoint = new List<Transform>();

    public override void StateEnter()
    {
        FindMovePosition();
    }

    public override void StateExit()
    {
        nomalHilichurl.SetDestination_This();
    }

    public override void StateUpDate()
    {
        nomalHilichurl.Trace();

        if(nomalHilichurl.Agent.remainingDistance <= nomalHilichurl.Agent.stoppingDistance)
        {
            nomalHilichurl.State.ChangeState(EnemyState.Idle);
        }
    }
    private void FindMovePosition()
    {
        GameObject movePoint = nomalHilichurl.transform.parent.gameObject;

        foreach (Transform point in movePoint.transform)
        {
            WayPoint.Add(point);
        }

        nomalHilichurl.Agent.SetDestination(WayPoint[Random.Range(0, WayPoint.Count)].transform.position);

        nomalHilichurl.MoveAnimation(3f);
    }
}

public class NomalHilichurlTraceMove : NomalHilichurlState
{
    public NomalHilichurlTraceMove(NomalHilichurl nomalHilichurl) : base(nomalHilichurl) { }

    public override void StateEnter()
    {
        nomalHilichurl.SetDestination_Player();
        nomalHilichurl.MoveAnimation(4f);
    }

    public override void StateExit()
    {
        nomalHilichurl.SetDestination_This();
        nomalHilichurl.MoveAnimation(0f);
    }

    public override void StateUpDate()
    {
        if (nomalHilichurl.Distance() > nomalHilichurl.Agent.stoppingDistance)
        {
            nomalHilichurl.SetDestination_Player();
        }
        else if (nomalHilichurl.Distance() <= nomalHilichurl.Agent.stoppingDistance)
        {
            nomalHilichurl.State.ChangeState(EnemyState.TraceAttack);
        }

        StopTracking();
    }

    private void StopTracking()
    {
        if (nomalHilichurl.Distance() > 15.0f)
        {
            nomalHilichurl.State.ChangeState(EnemyState.Move);
        }
            
    }
}

public class NomalHilichurlTraceAttack : NomalHilichurlState
{
    public NomalHilichurlTraceAttack(NomalHilichurl nomalHilichurl) : base(nomalHilichurl) { }
 
    public override void StateEnter()
    {
        nomalHilichurl.Agent.updateRotation = false;

        nomalHilichurl.SetDestination_This();

        nomalHilichurl.MonsterWeapon.SetAttackPower(nomalHilichurl.EnemyData.AttackPower);
    }

    public override void StateExit()
    {
        nomalHilichurl.Agent.updateRotation = true;

        nomalHilichurl.TraceAttack = true;
    }

    public override void StateUpDate()
    {
        if (nomalHilichurl.TraceAttack)
        {
            nomalHilichurl.TraceAttack = false;
            nomalHilichurl.Animator.SetTrigger("Attack");
            nomalHilichurl.MonsterWeapon.EableSword();
        }

        nomalHilichurl.TraceAttackRotation();
    }
}
