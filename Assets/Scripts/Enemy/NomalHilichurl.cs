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
    }

    public EnemyStateMachine State => state;
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
    float timer;
    public NomalHilichurlIdle(NomalHilichurl nomalHilichurl) : base(nomalHilichurl) { }
    
    public override void OnCollisionEnter(Collision collision)
    {
        
    }

    public override void StateEnter()
    {
        timer = 0f;
    }

    public override void StateExit()
    {
        
    }

    public override void StateUpDate()
    {
        timer += Time.deltaTime;

        if(timer > 4.0f)
        {
            nomalHilichurl.State.ChangeState(EnemyState.Move);
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
    }

    public override void StateExit()
    {
        agent.SetDestination(agent.transform.position);
    }

    public override void StateUpDate()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
            agent.SetDestination(WayPoint[Random.Range(0, WayPoint.Count)].transform.position);

        timer += Time.deltaTime;
        
        if(timer > 10f)
        {
            nomalHilichurl.State.ChangeState(EnemyState.Idle);
        }
    }
}

public class NomalHilichurlTrace : NomalHilichurlState
{
    public NomalHilichurlTrace(NomalHilichurl nomalHilichurl) : base(nomalHilichurl) { }
    
    public override void OnCollisionEnter(Collision collision)
    {
        
    }

    public override void StateEnter()
    {
        
    }

    public override void StateExit()
    {
        
    }

    public override void StateUpDate()
    {
        
    }
}
