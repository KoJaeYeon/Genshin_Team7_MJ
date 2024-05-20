using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class IceHilichurl : Enemy, IColor
{
    protected override void Awake()
    {
        base.Awake();

        state = gameObject.AddComponent<EnemyStateMachine>();
        state.AddState(EnemyState.Idle, new IceHilichurlIdle(this));
        state.AddState(EnemyState.Move, new IceHilichurlMove(this));
        state.AddState(EnemyState.TraceMove, new IceHilichurlTraceMove(this));
        State.AddState(EnemyState.TraceAttack,new IceHilichurlTraceAttack(this));
        //체력 , 공격력, 이동속도, 물리내성, 경험치 , 속성
        enemyData = new EnemyData(110f, 15f, 2f, 0.1f, 130, Element.Ice);
        EnemyHealthDic.Add(this, enemyData.Health);

        HpSlider.maxValue = enemyData.Health;
        HpSlider.value = enemyData.Health;
        //traceDistance = 7.0f;
    }

    public EnemyStateMachine State => state;
    public Animator Animator => animator;
    public Transform PlayerTransform => Player;
    public MonsterWeapon MonsterWeapon => Weapon;
    public NavMeshAgent Agent => agent;
    public float TraceDistance => traceDistance;
    public EnemyData EnemyData => enemyData;
    private Color color = Color.blue;
    public bool TraceAttack
    {
        get { return attack; }
        set { attack = value; }
    }

    public void OnAnimationEnd()
    {
        if (Vector3.Distance(transform.position, Player.position) > Agent.stoppingDistance)
        {
            State.ChangeState(EnemyState.TraceMove);
        }

        attack = true;
    }

    public Color GetColor()
    {
        return color;
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("HitObject") && gameObject.layer == (int)EnemyLayer.isAlive)
    //    {
    //        TestElement hitObject = other.GetComponent<TestElement>();
    //        HitElement = hitObject.GetElement();
    //        Damaged(this, hitObject.ReturnDamage(), HitElement);

    //        hitObject.Return();
    //    }
    //}

    public override void Damaged(Enemy enemy, float damage, Element element)
    {
        EnemyHealthDic[this] -= Armor(enemy, damage, element);
        HpSlider.value = EnemyHealthDic[this];
        transform.LookAt(Player.position);
        animator.SetTrigger("Hit");

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
            iceHilichurl.State.ChangeState(EnemyState.TraceMove);
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
            iceHilichurl.State.ChangeState(EnemyState.TraceMove);
        }
    }
}

public class IceHilichurlTraceMove : IceHilichurlState
{
 
    public IceHilichurlTraceMove(IceHilichurl iceHilichurl) : base(iceHilichurl) { }
    
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
        iceHilichurl.Animator.SetFloat("Move", 0);
    }

    public override void StateUpDate()
    {
        if (Vector3.Distance(iceHilichurl.PlayerTransform.position, iceHilichurl.transform.position) > iceHilichurl.Agent.stoppingDistance)
        {
            iceHilichurl.Agent.SetDestination(iceHilichurl.PlayerTransform.position);
        }
        else if (Vector3.Distance(iceHilichurl.PlayerTransform.position, iceHilichurl.transform.position) <= iceHilichurl.Agent.stoppingDistance)
        {
            iceHilichurl.State.ChangeState(EnemyState.TraceAttack);
        }

        StopTracking();
    }

    public void StopTracking()
    {
        if (Vector3.Distance(iceHilichurl.transform.position, iceHilichurl.PlayerTransform.position) > iceHilichurl.TraceDistance)
            iceHilichurl.State.ChangeState(EnemyState.Move);
    }

    private IEnumerator Jump()
    {
        iceHilichurl.TraceAttack = false;

        iceHilichurl.Agent.SetDestination(iceHilichurl.transform.position);

        yield return new WaitForSeconds(1.0f);

        iceHilichurl.Animator.SetTrigger("JumpAttack");
    }
}

public class IceHilichurlTraceAttack : IceHilichurlState
{
    private MonsterWeapon Weapon;
    public IceHilichurlTraceAttack(IceHilichurl iceHilichurl) : base(iceHilichurl) { }
    
    public override void OnCollisionEnter(Collision collision)
    {
        
    }

    public override void StateEnter()
    {
        iceHilichurl.Agent.updateRotation = false;
        iceHilichurl.Agent.SetDestination(iceHilichurl.transform.position);

        Weapon = iceHilichurl.transform.GetComponentInChildren<MonsterWeapon>();
        Weapon.SetAttackPower(iceHilichurl.EnemyData.AttackPower);
    }

    public override void StateExit()
    {
        iceHilichurl.Agent.updateRotation = true;
        iceHilichurl.TraceAttack = true;
    }

    public override void StateUpDate()
    {
        if (iceHilichurl.TraceAttack)
        {
            iceHilichurl.Animator.SetTrigger("GroundAttack");
            iceHilichurl.TraceAttack = false;
            Weapon.EableSword();
        }

        Vector3 direction = iceHilichurl.PlayerTransform.position - iceHilichurl.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        iceHilichurl.transform.rotation = rotation;
    }

    
}