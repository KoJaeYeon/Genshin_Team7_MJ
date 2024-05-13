using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireHilichurl : Enemy
{
    protected override void Awake()
    {
        base.Awake();
 
        state = gameObject.AddComponent<EnemyStateMachine>();
        state.AddState(EnemyState.Idle, new FireHilichurlIdle(this));
        state.AddState(EnemyState.Move, new FireHilichurlMove(this));
        state.AddState(EnemyState.Trace, new FireHilichurlTrace(this));
        //체력 , 공격력, 이동속도, 물리내성, 경험치 , 속성
        enemyData = new EnemyData(80f, 20f, 3f, 0.1f, 180, Element.Fire);
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

    public void OnAnimationEnd()
    {
        Debug.Log("호출");
        Vector3 direction = PlayerTransform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
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
    float timer = 0f;
    NavMeshAgent agent;
    public FireHilichurlIdle(FireHilichurl fireHilichurl) : base(fireHilichurl) { }

    public override void OnCollisionEnter(Collision collision) { }

    public override void StateEnter()
    {
        agent = fireHilichurl.gameObject.GetComponent<NavMeshAgent>();
        fireHilichurl.Animator.SetFloat("Move", 0f);
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
            fireHilichurl.State.ChangeState(EnemyState.Move);
        }
    }

    private void Trace()
    {
        if (Vector3.Distance(fireHilichurl.PlayerTransform.position, fireHilichurl.transform.position) <= fireHilichurl.TraceDistance)
        {
            fireHilichurl.State.ChangeState(EnemyState.Trace);
        }
    }
}

public class FireHilichurlMove : FireHilichurlState //이동 (배회)
{
    List<Transform> WayPoint = new List<Transform>();
    NavMeshAgent agent;
    public FireHilichurlMove(FireHilichurl fireHilichurl) : base(fireHilichurl) { }

    public override void OnCollisionEnter(Collision collision)
    {

    }

    public override void StateEnter()
    {
        agent = fireHilichurl.gameObject.GetComponent<NavMeshAgent>();

        //GameObject movePoint = GameObject.FindWithTag("WayPoint");

        GameObject movePoint = fireHilichurl.transform.parent.gameObject;

        foreach (Transform point in movePoint.transform)
        {
            WayPoint.Add(point);
        }

        agent.SetDestination(WayPoint[Random.Range(0, WayPoint.Count)].transform.position);
        fireHilichurl.Animator.SetFloat("Move", agent.speed);
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
            fireHilichurl.State.ChangeState(EnemyState.Idle);
        }
    }

    private void Trace()
    {
        if (Vector3.Distance(fireHilichurl.PlayerTransform.position, fireHilichurl.transform.position) <= fireHilichurl.TraceDistance)
        {
            fireHilichurl.State.ChangeState(EnemyState.Trace);
        }
    }
}

public class FireHilichurlTrace : FireHilichurlState //이동 (추적)
{
    NavMeshAgent agent;

    public FireHilichurlTrace(FireHilichurl fireHilichurl) : base(fireHilichurl) { }

    public override void OnCollisionEnter(Collision collision)
    {

    }

    public override void StateEnter()
    {
        agent = fireHilichurl.gameObject.GetComponent<NavMeshAgent>();
        agent.SetDestination(fireHilichurl.PlayerTransform.position);
        fireHilichurl.Animator.SetFloat("Move", agent.speed + 1);
    }

    public override void StateExit()
    {
        agent.SetDestination(fireHilichurl.transform.position);
    }

    public override void StateUpDate()
    {
        if (fireHilichurl.TraceMove)
        {
            if (Vector3.Distance(fireHilichurl.PlayerTransform.position, fireHilichurl.transform.position) > agent.stoppingDistance)
            {   
                fireHilichurl.Animator.SetBool("isAttack", false);
                agent.SetDestination(fireHilichurl.PlayerTransform.position);
            }
            else if (Vector3.Distance(fireHilichurl.PlayerTransform.position, fireHilichurl.transform.position) <= agent.stoppingDistance)
            {
                fireHilichurl.TraceMove = false;
            }
        }
        else
        {
            fireHilichurl.Animator.SetBool("isAttack", true);
        }

        StopTracking();
    }

    public void StopTracking()
    {
        if (Vector3.Distance(fireHilichurl.transform.position, fireHilichurl.PlayerTransform.position) > 15.0f)
            fireHilichurl.State.ChangeState(EnemyState.Move);
    }

}