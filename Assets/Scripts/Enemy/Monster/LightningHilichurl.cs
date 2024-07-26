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
        //체력 , 공격력, 이동속도, 물리내성, 경험치 , 속성
        enemyData = new EnemyData(120f, 20f, 5f, 0.5f, 180, Element.Lightning);
        EnemyHealthDic.Add(this, enemyData.Health);

        HpSlider.maxValue = enemyData.Health;
        HpSlider.value = enemyData.Health;
    }

    private Color color = Color.yellow;
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

    //AnimationEvent---------------------------------------------
    public void OnAnimationEnd()
    {
        if (Distance() > Agent.stoppingDistance)
        {
            State.ChangeState(EnemyState.TraceMove);
        }

        attack = true;

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

        if (timer > 4.0f)
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
        if (lightningHilichurl.Distance() > 15.0f)
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

        lightningHilichurl.MonsterWeapon.SetAttackPower(lightningHilichurl.EnemyData.AttackPower);
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
            lightningHilichurl.MonsterWeapon.EableSword();
        }

        lightningHilichurl.TraceAttackRotation();
    }
}