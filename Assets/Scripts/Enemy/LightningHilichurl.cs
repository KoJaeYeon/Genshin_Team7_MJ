using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LightningHilichurl : Enemy
{
    protected override void Awake()
    {
        base.Awake();

        state = gameObject.AddComponent<EnemyStateMachine>();
        state.AddState(EnemyState.Idle, new LightningHilichurlIdle(this));
        state.AddState(EnemyState.Move, new LightningHilichurlMove(this));
        state.AddState(EnemyState.Trace, new LightningHilichurlTrace(this));
        //체력 , 공격력, 이동속도, 물리내성, 경험치 , 속성
        enemyData = new EnemyData(120f, 20f, 5f, 0.5f, 180, Element.Lightning);
        EnemyHealthDic.Add(this, enemyData.Health);
        animator = GetComponent<Animator>();
    }
    public EnemyStateMachine State => state;
    public Animator Animator => animator;
    public Transform PlayerTransform => Player;
    public MonsterWeapon MonsterWeapon => Weapon;
    public float TraceDistance => traceDistance;
    public bool TraceMove
    {
        get { return traceMove; }
        set { traceMove = value; }
    }

    public void IsMove() //Animation Event
    {
        if (Vector3.Distance(transform.position, Player.position) > 2.0f)
            traceMove = true;
    }

    public void UseWeapon() //Animation Event
    {
        Weapon.EableSword();
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
    float timer = 0f;
    NavMeshAgent agent;
    public LightningHilichurlIdle(LightningHilichurl lightningHilichurl) : base(lightningHilichurl) { }

    public override void OnCollisionEnter(Collision collision) { }

    public override void StateEnter()
    {
        agent = lightningHilichurl.gameObject.GetComponent<NavMeshAgent>();
        lightningHilichurl.Animator.SetFloat("Move", 0f);
        lightningHilichurl.MonsterWeapon.DisableSword();
    }

    public override void StateExit()
    {
        timer = 0f;
    }

    public override void StateUpDate()
    {
        Trace();

        timer += Time.deltaTime;

        if (timer > 4.0f)
        {
            lightningHilichurl.State.ChangeState(EnemyState.Move);
        }
    }

    private void Trace()
    {
        if (Vector3.Distance(lightningHilichurl.PlayerTransform.position, lightningHilichurl.transform.position) <= lightningHilichurl.TraceDistance)
        {
            lightningHilichurl.State.ChangeState(EnemyState.Trace);
        }
    }
}

public class LightningHilichurlMove : LightningHilichurlState //이동 (배회)
{
    float timer;
    List<Transform> WayPoint = new List<Transform>();
    NavMeshAgent agent;
    public LightningHilichurlMove(LightningHilichurl lightningHilichurl) : base(lightningHilichurl) { }

    public override void OnCollisionEnter(Collision collision)
    {

    }

    public override void StateEnter()
    {
        agent = lightningHilichurl.gameObject.GetComponent<NavMeshAgent>();
        timer = 0f;
        GameObject movePoint = GameObject.FindWithTag("WayPoint");

        foreach (Transform point in movePoint.transform)
        {
            WayPoint.Add(point);
        }

        agent.SetDestination(WayPoint[Random.Range(0, WayPoint.Count)].transform.position);
        lightningHilichurl.Animator.SetFloat("Move", agent.speed);
    }

    public override void StateExit()
    {
        agent.SetDestination(agent.transform.position);
    }

    public override void StateUpDate()
    {
        Trace();

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            lightningHilichurl.State.ChangeState(EnemyState.Idle);
        }
    }

    private void Trace()
    {
        if (Vector3.Distance(lightningHilichurl.PlayerTransform.position, lightningHilichurl.transform.position) <= lightningHilichurl.TraceDistance)
        {
            lightningHilichurl.State.ChangeState(EnemyState.Trace);
        }
    }
}

public class LightningHilichurlTrace : LightningHilichurlState //이동 (추적)
{
    NavMeshAgent agent;

    public LightningHilichurlTrace(LightningHilichurl lightningHilichurl) : base(lightningHilichurl) { }

    public override void OnCollisionEnter(Collision collision)
    {

    }

    public override void StateEnter()
    {
        agent = lightningHilichurl.gameObject.GetComponent<NavMeshAgent>();
        agent.SetDestination(lightningHilichurl.PlayerTransform.position);
        lightningHilichurl.Animator.SetFloat("Move", agent.speed + 1);
    }

    public override void StateExit()
    {
        agent.SetDestination(lightningHilichurl.transform.position);
    }

    public override void StateUpDate()
    {
        if (lightningHilichurl.TraceMove)
        {
            if (Vector3.Distance(lightningHilichurl.PlayerTransform.position, lightningHilichurl.transform.position) > agent.stoppingDistance)
            {
                lightningHilichurl.Animator.SetBool("isAttack", false);
                agent.SetDestination(lightningHilichurl.PlayerTransform.position);
            }
            else if (Vector3.Distance(lightningHilichurl.PlayerTransform.position, lightningHilichurl.transform.position) <= agent.stoppingDistance)
            {
                lightningHilichurl.TraceMove = false;
            }
        }
        else
        {
            lightningHilichurl.Animator.SetBool("isAttack", true);
        }

        StopTracking();
    }

    public void StopTracking()
    {
        if (Vector3.Distance(lightningHilichurl.transform.position, lightningHilichurl.PlayerTransform.position) > 15.0f)
            lightningHilichurl.State.ChangeState(EnemyState.Move);
    }

}