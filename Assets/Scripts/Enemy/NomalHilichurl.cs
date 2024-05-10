using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NomalHilichurl : Enemy
{
    protected override void Awake()
    {
        base.Awake();

        state = gameObject.AddComponent<EnemyStateMachine>();
        state.AddState(EnemyState.Idle, new NomalHilichurlIdle(this));
        state.AddState(EnemyState.Move, new NomalHilichurlMove(this));
        state.AddState(EnemyState.Trace, new NomalHilichurlTrace(this));

        enemyData = new EnemyData(100f, 10f, 10f, 0.1f, 100);
        EnemyHealthDic.Add(this, enemyData.Health);
        animator = GetComponent<Animator>();
    }

    public EnemyStateMachine State => state;
    public Animator Animator => animator;
    public float TraceDistance => traceDistance;
}

public abstract class NomalHilichurlState : BaseState
{
    protected NomalHilichurl nomalHilichurl;
    public NomalHilichurlState(NomalHilichurl nomalHilichurl)
    {
        this.nomalHilichurl = nomalHilichurl;
    }
}

public class NomalHilichurlIdle : NomalHilichurlState
{
    float timer = 0f;
    NavMeshAgent agent;
    Transform Player;
    public NomalHilichurlIdle(NomalHilichurl nomalHilichurl) : base(nomalHilichurl) { }
    
    public override void OnCollisionEnter(Collision collision)
    {
        
    }

    public override void StateEnter()
    {
        agent = nomalHilichurl.gameObject.GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        nomalHilichurl.Animator.SetFloat("Move", 0f);
    }

    public override void StateExit()
    {
        timer = 0f;
    }

    public override void StateUpDate()
    {
        Trace();

        timer += Time.deltaTime;

        if(timer > 4.0f)
        {
            nomalHilichurl.State.ChangeState(EnemyState.Move);
        }
    }

    private void Trace()
    {
        if (Vector3.Distance(Player.position, nomalHilichurl.transform.position) <= nomalHilichurl.TraceDistance)
            nomalHilichurl.State.ChangeState(EnemyState.Trace);
    }
}

public class NomalHilichurlMove : NomalHilichurlState
{
    float timer;
    List<Transform> WayPoint = new List<Transform>();
    NavMeshAgent agent;
    public NomalHilichurlMove(NomalHilichurl nomalHilichurl) : base(nomalHilichurl) { }
   
    public override void OnCollisionEnter(Collision collision)
    {
        
    }

    public override void StateEnter()
    {
       
        agent = nomalHilichurl.gameObject.GetComponent<NavMeshAgent>();
        timer = 0f;
        GameObject movePoint = GameObject.FindWithTag("WayPoint");

        foreach (Transform point in movePoint.transform)
        {
            WayPoint.Add(point);
        }

        agent.SetDestination(WayPoint[Random.Range(0, WayPoint.Count)].transform.position);
        nomalHilichurl.Animator.SetFloat("Move", agent.speed);
    }

    public override void StateExit()
    {
        agent.SetDestination(agent.transform.position);
    }

    public override void StateUpDate()
    {
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            nomalHilichurl.State.ChangeState(EnemyState.Idle);
        }
    }
}

public class NomalHilichurlTrace : NomalHilichurlState
{
    Transform Player;
    NavMeshAgent agent;

    private bool isMove = true;
    public NomalHilichurlTrace(NomalHilichurl nomalHilichurl) : base(nomalHilichurl) { }
    
    public override void OnCollisionEnter(Collision collision)
    {
        
    }

    public override void StateEnter()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        agent = nomalHilichurl.gameObject.GetComponent<NavMeshAgent>();
        nomalHilichurl.Animator.SetFloat("Move", agent.speed + 1);
    }

    public override void StateExit()
    {
        agent.SetDestination(nomalHilichurl.transform.position);
    }

    public override void StateUpDate()
    {
        agent.SetDestination(Player.position);
        Attack();

    }

    private void Attack()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
            nomalHilichurl.Animator.SetBool("isAttack", true);
        else
            nomalHilichurl.Animator.SetFloat("Move", agent.speed + 1);
    }
}
