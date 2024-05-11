using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public Transform PlayerTransform => Player;
    public float TraceDistance => traceDistance;
    public bool TraceMove
    {
        get { return traceMove; }
        set { traceMove = value; }
    }

    public void IsMove() //Animation Event
    {
        if(Vector3.Distance(transform.position, Player.position) > 2.0f)
            traceMove = true;
    }

    public void UseWeapon()
    {
        Weapon.UseSword();
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

public class NomalHilichurlIdle : NomalHilichurlState
{
    float timer = 0f;
    NavMeshAgent agent;
    public NomalHilichurlIdle(NomalHilichurl nomalHilichurl) : base(nomalHilichurl) { }
    
    public override void OnCollisionEnter(Collision collision)
    {
        
    }

    public override void StateEnter()
    {
        agent = nomalHilichurl.gameObject.GetComponent<NavMeshAgent>();
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
        if (Vector3.Distance(nomalHilichurl.PlayerTransform.position, nomalHilichurl.transform.position) <= nomalHilichurl.TraceDistance)
        {
            nomalHilichurl.State.ChangeState(EnemyState.Trace);
        }
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
        Trace();

        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            nomalHilichurl.State.ChangeState(EnemyState.Idle);
        }
    }

    private void Trace()
    {
        if (Vector3.Distance(nomalHilichurl.PlayerTransform.position, nomalHilichurl.transform.position) <= nomalHilichurl.TraceDistance)
        {
            nomalHilichurl.State.ChangeState(EnemyState.Trace);
        }
    }
}

public class NomalHilichurlTrace : NomalHilichurlState
{
    NavMeshAgent agent;

    public NomalHilichurlTrace(NomalHilichurl nomalHilichurl) : base(nomalHilichurl) { }
    
    public override void OnCollisionEnter(Collision collision)
    {
        
    }

    public override void StateEnter()
    {
        agent = nomalHilichurl.gameObject.GetComponent<NavMeshAgent>();
        agent.SetDestination(nomalHilichurl.PlayerTransform.position);
        nomalHilichurl.Animator.SetFloat("Move", agent.speed + 1);
    }

    public override void StateExit()
    {
        agent.SetDestination(nomalHilichurl.transform.position);
    }

    public override void StateUpDate()
    {
        if (nomalHilichurl.TraceMove)
        {
            if(Vector3.Distance(nomalHilichurl.PlayerTransform.position,nomalHilichurl.transform.position) > agent.stoppingDistance)
            {
                nomalHilichurl.Animator.SetBool("isAttack", false);
                agent.SetDestination(nomalHilichurl.PlayerTransform.position);
            }
            else if(Vector3.Distance(nomalHilichurl.PlayerTransform.position, nomalHilichurl.transform.position) <= agent.stoppingDistance)
            {
                nomalHilichurl.TraceMove = false;
            }
        }
        else
        {
            nomalHilichurl.Animator.SetBool("isAttack", true);
        }

        StopTracking();
    }

    public void StopTracking()
    {
        if (Vector3.Distance(nomalHilichurl.transform.position, nomalHilichurl.PlayerTransform.position) > 20.0f)
            nomalHilichurl.State.ChangeState(EnemyState.Move);
    }

}
