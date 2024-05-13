using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class IceHilichurl : Enemy
{
    protected override void Awake()
    {
        base.Awake();

        state = gameObject.AddComponent<EnemyStateMachine>();
        state.AddState(EnemyState.Idle, new IceHilichurlIdle(this));
        state.AddState(EnemyState.Move, new IceHilichurlMove(this));
        state.AddState(EnemyState.Trace, new IceHilichurlTrace(this));
        //ü�� , ���ݷ�, �̵��ӵ�, ��������, ����ġ , �Ӽ�
        enemyData = new EnemyData(110f, 15f, 2f, 0.1f, 130, Element.Ice);
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
    float timer = 0f;
    NavMeshAgent agent;
    public IceHilichurlIdle(IceHilichurl iceHilichurl) : base(iceHilichurl) { }

    public override void OnCollisionEnter(Collision collision)
    {
        
    }

    public override void StateEnter()
    {
        agent = iceHilichurl.gameObject.GetComponent<NavMeshAgent>();
        iceHilichurl.Animator.SetFloat("Move", 0f);
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
            iceHilichurl.State.ChangeState(EnemyState.Move);
        }
    }
    private void Trace()
    {
        if (Vector3.Distance(iceHilichurl.PlayerTransform.position, iceHilichurl.transform.position) <= iceHilichurl.TraceDistance)
        {
            iceHilichurl.State.ChangeState(EnemyState.Trace);
        }
    }
}

public class IceHilichurlMove : IceHilichurlState
{
    
    List<Transform> WayPoint = new List<Transform>();
    NavMeshAgent agent;
    public IceHilichurlMove(IceHilichurl iceHilichurl) : base(iceHilichurl) { }
    
    public override void OnCollisionEnter(Collision collision)
    {
        
    }

    public override void StateEnter()
    {
        agent = iceHilichurl.gameObject.GetComponent<NavMeshAgent>();
        
        GameObject movePoint = GameObject.FindWithTag("WayPoint");

        foreach (Transform point in movePoint.transform)
        {
            WayPoint.Add(point);
        }

        agent.SetDestination(WayPoint[Random.Range(0, WayPoint.Count)].transform.position);
        iceHilichurl.Animator.SetFloat("Move", agent.speed);
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
            iceHilichurl.State.ChangeState(EnemyState.Idle);
        }
    }

    private void Trace()
    {
        if (Vector3.Distance(iceHilichurl.PlayerTransform.position, iceHilichurl.transform.position) <= iceHilichurl.TraceDistance)
        {
            iceHilichurl.State.ChangeState(EnemyState.Trace);
        }
    }
}

public class IceHilichurlTrace : IceHilichurlState
{
    NavMeshAgent agent;
    public IceHilichurlTrace(IceHilichurl iceHilichurl) : base(iceHilichurl) { }
    
    public override void OnCollisionEnter(Collision collision)
    {
        
    }

    public override void StateEnter()
    {
        agent = iceHilichurl.gameObject.GetComponent<NavMeshAgent>();
        agent.SetDestination(iceHilichurl.PlayerTransform.position);
        iceHilichurl.Animator.SetFloat("Move", agent.speed + 1);
    }

    public override void StateExit()
    {
        agent.SetDestination(iceHilichurl.transform.position);
    }

    public override void StateUpDate()
    {
        if (iceHilichurl.TraceMove)
        {
            if (Vector3.Distance(iceHilichurl.PlayerTransform.position, iceHilichurl.transform.position) > agent.stoppingDistance)
            {
                iceHilichurl.Animator.SetBool("isAttack", false);
                agent.SetDestination(iceHilichurl.PlayerTransform.position);
            }
            else if (Vector3.Distance(iceHilichurl.PlayerTransform.position, iceHilichurl.transform.position) <= agent.stoppingDistance)
            {
                iceHilichurl.TraceMove = false;
            }
        }
        else
        {
            iceHilichurl.Animator.SetBool("isAttack", true);
        }

        StopTracking();
    }

    public void StopTracking()
    {
        if (Vector3.Distance(iceHilichurl.transform.position, iceHilichurl.PlayerTransform.position) > 15.0f)
            iceHilichurl.State.ChangeState(EnemyState.Move);
    }
}