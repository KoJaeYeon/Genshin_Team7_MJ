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

        state = gameObject.AddComponent<EnemyStateMachine>();
        state.AddState(EnemyState.Idle, new NomalHilichurlIdle(this));
        state.AddState(EnemyState.Move, new NomalHilichurlMove(this));
        state.AddState(EnemyState.TraceMove, new NomalHilichurlTraceMove(this));
        state.AddState(EnemyState.TraceAttack,new NomalHilichurlTraceAttack(this)); 
                        //체력 , 공격력, 이동속도, 물리내성, 경험치 , 속성
        enemyData = new EnemyData(100f, 10f, 3f, 0.1f, 100, Element.Nomal);
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
    private Color color = Color.white;
    public bool TraceAttack
    {
        get { return attack; }
        set { attack = value; }
    }
    public void OnAnimationEnd()
    {
        if (Vector3.Distance(transform.position, Player.position) > Agent.stoppingDistance)
            state.ChangeState(EnemyState.TraceMove);

        attack = true;
    }

    public Color GetColor()
    {
        return color;   
    }

    public override void TakeDamage(float damage, Element element)
    {
        EnemyHealthDic[this] -= CalculateDamage(damage, element);
        HpSlider.value = EnemyHealthDic[this];
        transform.LookAt(Player.position);
        animator.SetTrigger("Hit");
        UIManager.Instance.DamageText(damage);

        if (EnemyHealthDic[this] <= 0)
        {
            Hp.SetActive(false);
            StartCoroutine(Die(this));
        }
        else
            HitDropElement(element);
    }

    public override void Splash(float damage)
    {
        EnemyHealthDic[this] -= damage;
        HpSlider.value = EnemyHealthDic[this];
        if (EnemyHealthDic[this] <= 0)
        {
            StartCoroutine(Die(this));
        }
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
    private float timer = 0f;    
    public NomalHilichurlIdle(NomalHilichurl nomalHilichurl) : base(nomalHilichurl) { }
    
    public override void OnCollisionEnter(Collision collision) { }
   
    public override void StateEnter()
    {
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
            nomalHilichurl.State.ChangeState(EnemyState.TraceMove);
        }
    }
}

public class NomalHilichurlMove : NomalHilichurlState //이동 (배회)
{
    List<Transform> WayPoint = new List<Transform>();
    public NomalHilichurlMove(NomalHilichurl nomalHilichurl) : base(nomalHilichurl) { }
   
    public override void OnCollisionEnter(Collision collision)
    {
     
    }

    public override void StateEnter()
    {
        GameObject movePoint = nomalHilichurl.transform.parent.gameObject;

        foreach (Transform point in movePoint.transform)
        {
            WayPoint.Add(point);
        }

        nomalHilichurl.Agent.SetDestination(WayPoint[Random.Range(0, WayPoint.Count)].transform.position);
        nomalHilichurl.Animator.SetFloat("Move", nomalHilichurl.Agent.speed);
    }

    public override void StateExit()
    {
        nomalHilichurl.Agent.SetDestination(nomalHilichurl.Agent.transform.position);
    }

    public override void StateUpDate()
    {
        Trace();

        if(nomalHilichurl.Agent.remainingDistance <= nomalHilichurl.Agent.stoppingDistance)
        {
            nomalHilichurl.State.ChangeState(EnemyState.Idle);
        }
    }

    private void Trace()
    {
        if (Vector3.Distance(nomalHilichurl.PlayerTransform.position, nomalHilichurl.transform.position) <= nomalHilichurl.TraceDistance)
        {
            nomalHilichurl.State.ChangeState(EnemyState.TraceMove);
        }
    }
}

public class NomalHilichurlTraceMove : NomalHilichurlState
{
    public NomalHilichurlTraceMove(NomalHilichurl nomalHilichurl) : base(nomalHilichurl) { }
    
    public override void OnCollisionEnter(Collision collision)
    {
     
    }

    public override void StateEnter()
    {
        nomalHilichurl.Agent.SetDestination(nomalHilichurl.PlayerTransform.position);
        nomalHilichurl.Animator.SetFloat("Move", nomalHilichurl.Agent.speed + 1);
    }

    public override void StateExit()
    {
        nomalHilichurl.Agent.SetDestination(nomalHilichurl.transform.position);
        nomalHilichurl.Animator.SetFloat("Move", 0);
    }

    public override void StateUpDate()
    {
        if (Vector3.Distance(nomalHilichurl.PlayerTransform.position, nomalHilichurl.transform.position) > nomalHilichurl.Agent.stoppingDistance)
        {
            nomalHilichurl.Agent.SetDestination(nomalHilichurl.PlayerTransform.position);
        }
        else if (Vector3.Distance(nomalHilichurl.PlayerTransform.position, nomalHilichurl.transform.position) <= nomalHilichurl.Agent.stoppingDistance)
        {
            nomalHilichurl.State.ChangeState(EnemyState.TraceAttack);
        }

        StopTracking();
    }

    public void StopTracking()
    {
        if (Vector3.Distance(nomalHilichurl.transform.position, nomalHilichurl.PlayerTransform.position) > 15.0f)
            nomalHilichurl.State.ChangeState(EnemyState.Move);
    }

}

public class NomalHilichurlTraceAttack : NomalHilichurlState
{
    private MonsterWeapon Weapon;
    public NomalHilichurlTraceAttack(NomalHilichurl nomalHilichurl) : base(nomalHilichurl) { }
    
    public override void OnCollisionEnter(Collision collision)
    {
        
    }

    public override void StateEnter()
    {
        nomalHilichurl.Agent.updateRotation = false;
        nomalHilichurl.Agent.SetDestination(nomalHilichurl.transform.position);

        Weapon = nomalHilichurl.transform.GetComponentInChildren<MonsterWeapon>();
        Weapon.SetAttackPower(nomalHilichurl.EnemyData.AttackPower);
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
            Weapon.EableSword();
        }

        Vector3 direction = nomalHilichurl.PlayerTransform.position - nomalHilichurl.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        nomalHilichurl.transform.rotation = rotation;
    }
}
