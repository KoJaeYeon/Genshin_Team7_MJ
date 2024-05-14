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
        //체력 , 공격력, 이동속도, 물리내성, 경험치 , 속성
        enemyData = new EnemyData(110f, 15f, 2f, 0.1f, 130, Element.Ice);
        EnemyHealthDic.Add(this, enemyData.Health);
    }

    public EnemyStateMachine State => state;
    public Animator Animator => animator;
    public Transform PlayerTransform => Player;
    public MonsterWeapon MonsterWeapon => Weapon;
    public NavMeshAgent Agent => agent;
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
    private float timer = 0f;
    public IceHilichurlIdle(IceHilichurl iceHilichurl) : base(iceHilichurl) { }

    public override void OnCollisionEnter(Collision collision)
    {
        
    }

    public override void StateEnter()
    {
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
    public IceHilichurlMove(IceHilichurl iceHilichurl) : base(iceHilichurl) { }
    
    public override void OnCollisionEnter(Collision collision)
    {
        
    }

    public override void StateEnter()
    {
        GameObject movePoint = iceHilichurl.transform.parent.gameObject;

        foreach (Transform point in movePoint.transform)
        {
            WayPoint.Add(point);
        }

        iceHilichurl.Agent.SetDestination(WayPoint[Random.Range(0, WayPoint.Count)].transform.position);
        iceHilichurl.Animator.SetFloat("Move", iceHilichurl.Agent.speed);
    }

    public override void StateExit()
    {
        iceHilichurl.Agent.SetDestination(iceHilichurl.Agent.transform.position);
    }

    public override void StateUpDate()
    {
        Trace();

        if (iceHilichurl.Agent.remainingDistance <= iceHilichurl.Agent.stoppingDistance)
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
    public IceHilichurlTrace(IceHilichurl iceHilichurl) : base(iceHilichurl) { }
    
    public override void OnCollisionEnter(Collision collision)
    {
        
    }

    public override void StateEnter()
    {
        iceHilichurl.Agent.SetDestination(iceHilichurl.PlayerTransform.position);
        iceHilichurl.Animator.SetFloat("Move", iceHilichurl.Agent.speed + 1);
    }

    public override void StateExit()
    {
        iceHilichurl.Agent.SetDestination(iceHilichurl.transform.position);
    }

    public override void StateUpDate()
    {
        if (iceHilichurl.TraceMove)
        {
            if (Vector3.Distance(iceHilichurl.PlayerTransform.position, iceHilichurl.transform.position) > iceHilichurl.Agent.stoppingDistance)
            {
                iceHilichurl.Animator.SetBool("isAttack", false);
                iceHilichurl.Agent.SetDestination(iceHilichurl.PlayerTransform.position);
            }
            else if (Vector3.Distance(iceHilichurl.PlayerTransform.position, iceHilichurl.transform.position) <= iceHilichurl.Agent.stoppingDistance)
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