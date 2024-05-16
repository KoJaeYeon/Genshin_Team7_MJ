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
        state.AddState(EnemyState.TraceMove, new LightningHilichurlTraceMove(this));
        state.AddState(EnemyState.TraceAttack,new LightningHilichurlTraceAttack(this));
        //체력 , 공격력, 이동속도, 물리내성, 경험치 , 속성
        enemyData = new EnemyData(120f, 20f, 5f, 0.5f, 180, Element.Lightning);
        EnemyHealthDic.Add(this, enemyData.Health);

        HpSlider.maxValue = enemyData.Health;
        HpSlider.value = enemyData.Health;
    }
    public EnemyStateMachine State => state;
    public Animator Animator => animator;
    public Transform PlayerTransform => Player;
    public MonsterWeapon MonsterWeapon => Weapon;
    public NavMeshAgent Agent => agent;
    public float TraceDistance => traceDistance;
    public EnemyData EnemyData => enemyData;
    
    public bool TraceAttack
    {
        get { return attack; }
        set { attack = value; }
    }

    public void OnAnimationEnd()
    {
        if (Vector3.Distance(transform.position, Player.position) > Agent.stoppingDistance)
            State.ChangeState(EnemyState.TraceMove);

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
    private float timer = 0f;
    public LightningHilichurlIdle(LightningHilichurl lightningHilichurl) : base(lightningHilichurl) { }

    public override void OnCollisionEnter(Collision collision) { }

    public override void StateEnter()
    {
        lightningHilichurl.Animator.SetFloat("Move", 0f);
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
            lightningHilichurl.State.ChangeState(EnemyState.TraceMove);
        }
    }
}

public class LightningHilichurlMove : LightningHilichurlState //이동 (배회)
{
    List<Transform> WayPoint = new List<Transform>();
    public LightningHilichurlMove(LightningHilichurl lightningHilichurl) : base(lightningHilichurl) { }

    public override void OnCollisionEnter(Collision collision)
    {

    }

    public override void StateEnter()
    {
        GameObject movePoint = lightningHilichurl.transform.parent.gameObject;

        foreach (Transform point in movePoint.transform)
        {
            WayPoint.Add(point);
        }

        lightningHilichurl.Agent.SetDestination(WayPoint[Random.Range(0, WayPoint.Count)].transform.position);
        lightningHilichurl.Animator.SetFloat("Move", lightningHilichurl.Agent.speed);
    }

    public override void StateExit()
    {
        lightningHilichurl.Agent.SetDestination(lightningHilichurl.Agent.transform.position);
    }

    public override void StateUpDate()
    {
        Trace();

        if (lightningHilichurl.Agent.remainingDistance <= lightningHilichurl.Agent.stoppingDistance)
        {
            lightningHilichurl.State.ChangeState(EnemyState.Idle);
        }
    }

    private void Trace()
    {
        if (Vector3.Distance(lightningHilichurl.PlayerTransform.position, lightningHilichurl.transform.position) <= lightningHilichurl.TraceDistance)
        {
            lightningHilichurl.State.ChangeState(EnemyState.TraceMove);
        }
    }
}

public class LightningHilichurlTraceMove : LightningHilichurlState //이동 (추적)
{
    public LightningHilichurlTraceMove(LightningHilichurl lightningHilichurl) : base(lightningHilichurl) { }

    public override void OnCollisionEnter(Collision collision)
    {

    }

    public override void StateEnter()
    {
        lightningHilichurl.Agent.SetDestination(lightningHilichurl.PlayerTransform.position);
        lightningHilichurl.Animator.SetFloat("Move", lightningHilichurl.Agent.speed + 1);
    }

    public override void StateExit()
    {
        lightningHilichurl.Agent.SetDestination(lightningHilichurl.transform.position);
        lightningHilichurl.Animator.SetFloat("Move", 0);
    }

    public override void StateUpDate()
    {
        if (Vector3.Distance(lightningHilichurl.PlayerTransform.position, lightningHilichurl.transform.position) > lightningHilichurl.Agent.stoppingDistance)
        {
            lightningHilichurl.Agent.SetDestination(lightningHilichurl.PlayerTransform.position);
        }
        else if (Vector3.Distance(lightningHilichurl.PlayerTransform.position, lightningHilichurl.transform.position) <= lightningHilichurl.Agent.stoppingDistance)
        {
            lightningHilichurl.State.ChangeState(EnemyState.TraceAttack);
        }

        StopTracking();
    }

    public void StopTracking()
    {
        if (Vector3.Distance(lightningHilichurl.transform.position, lightningHilichurl.PlayerTransform.position) > 15.0f)
            lightningHilichurl.State.ChangeState(EnemyState.Move);
    }

}

public class LightningHilichurlTraceAttack : LightningHilichurlState
{
    private MonsterWeapon Weapon;
    public LightningHilichurlTraceAttack(LightningHilichurl lightningHilichurl) : base(lightningHilichurl) { }
    
    public override void OnCollisionEnter(Collision collision)
    {
        
    }

    public override void StateEnter()
    {
        lightningHilichurl.Agent.updateRotation = false;
        lightningHilichurl.Agent.SetDestination(lightningHilichurl.transform.position);

        Weapon = lightningHilichurl.transform.GetComponentInChildren<MonsterWeapon>();
        Weapon.SetAttackPower(lightningHilichurl.EnemyData.AttackPower);
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
            Weapon.EableSword();
        }

        Vector3 direction = lightningHilichurl.PlayerTransform.position - lightningHilichurl.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        lightningHilichurl.transform.rotation = rotation;
    }
}