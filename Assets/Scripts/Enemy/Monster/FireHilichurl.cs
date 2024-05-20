using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireHilichurl : Enemy,IColor
{
    protected override void Awake()
    {
        base.Awake();
 
        state = gameObject.AddComponent<EnemyStateMachine>();
        state.AddState(EnemyState.Idle, new FireHilichurlIdle(this));
        state.AddState(EnemyState.Move, new FireHilichurlMove(this));
        state.AddState(EnemyState.TraceAttack, new FireHilichurlTraceAttack(this));
        state.AddState(EnemyState.TraceMove, new  FireHilichurlTraceMove(this));
        //체력 , 공격력, 이동속도, 물리내성, 경험치 , 속성
        enemyData = new EnemyData(80f, 20f, 3f, 0.1f, 180, Element.Fire);
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
    private Color color = Color.red;
    public bool TraceAttack
    {
        get { return  attack; }
        set { attack = value; }
    }
    public void OnAnimationEnd() //Animation Event
    { 
        if (Vector3.Distance(transform.position, Player.position) > Agent.stoppingDistance)
            State.ChangeState(EnemyState.TraceMove);

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
    private float timer = 0f;

    public FireHilichurlIdle(FireHilichurl fireHilichurl) : base(fireHilichurl) { }

    public override void OnCollisionEnter(Collision collision) { }

    public override void StateEnter()
    {
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
            fireHilichurl.State.ChangeState(EnemyState.TraceMove);
        }
    }
}

public class FireHilichurlMove : FireHilichurlState //이동 (배회)
{
    List<Transform> WayPoint = new List<Transform>();
    public FireHilichurlMove(FireHilichurl fireHilichurl) : base(fireHilichurl) { }

    public override void OnCollisionEnter(Collision collision) { }
   
    public override void StateEnter()
    {
        GameObject movePoint = fireHilichurl.transform.parent.gameObject; //WayPoint Transform

        foreach (Transform point in movePoint.transform)
        {
            WayPoint.Add(point);
        }

        fireHilichurl.Agent.SetDestination(WayPoint[Random.Range(0, WayPoint.Count)].transform.position);
        fireHilichurl.Animator.SetFloat("Move", fireHilichurl.Agent.speed);
    }

    public override void StateExit()
    {
        fireHilichurl.Agent.SetDestination(fireHilichurl.Agent.transform.position);
    }

    public override void StateUpDate()
    {
        Trace();

        if (fireHilichurl.Agent.remainingDistance <= fireHilichurl.Agent.stoppingDistance)
        {
            fireHilichurl.State.ChangeState(EnemyState.Idle);
        }
    }

    private void Trace()
    {
        if (Vector3.Distance(fireHilichurl.PlayerTransform.position, fireHilichurl.transform.position) <= fireHilichurl.TraceDistance)
        {
            fireHilichurl.State.ChangeState(EnemyState.TraceMove);
        }
    }
}

public class FireHilichurlTraceMove : FireHilichurlState //(추적 : 이동)
{
    public FireHilichurlTraceMove(FireHilichurl fireHilichurl) : base(fireHilichurl) { }

    public override void OnCollisionEnter(Collision collision) { }
    
    public override void StateEnter()
    {
        fireHilichurl.Agent.SetDestination(fireHilichurl.PlayerTransform.position);
        fireHilichurl.Animator.SetFloat("Move", fireHilichurl.Agent.speed + 1);
    }

    public override void StateExit()
    {
        fireHilichurl.Agent.SetDestination(fireHilichurl.transform.position);
        fireHilichurl.Animator.SetFloat("Move", 0);
    }

    public override void StateUpDate()
    {
        if (Vector3.Distance(fireHilichurl.PlayerTransform.position, fireHilichurl.transform.position) > fireHilichurl.Agent.stoppingDistance)
        {
            fireHilichurl.Agent.SetDestination(fireHilichurl.PlayerTransform.position);
        }
        else if(Vector3.Distance(fireHilichurl.PlayerTransform.position,fireHilichurl.transform.position) <= fireHilichurl.Agent.stoppingDistance)
        {
            fireHilichurl.State.ChangeState(EnemyState.TraceAttack);
        }

        StopTracking();
    }

    public void StopTracking()
    {
        if (Vector3.Distance(fireHilichurl.transform.position, fireHilichurl.PlayerTransform.position) > 15.0f)
            fireHilichurl.State.ChangeState(EnemyState.Move);
    }
}

public class FireHilichurlTraceAttack : FireHilichurlState //(추적 : 공격)
{
    private MonsterWeapon Weapon;
    public FireHilichurlTraceAttack(FireHilichurl fireHilichurl) : base(fireHilichurl) { }

    public override void OnCollisionEnter(Collision collision) { }
    
    public override void StateEnter()
    {
        fireHilichurl.Agent.updateRotation = false;
        fireHilichurl.Agent.SetDestination(fireHilichurl.transform.position);

        Weapon = fireHilichurl.transform.GetComponentInChildren<MonsterWeapon>();
        Weapon.SetAttackPower(fireHilichurl.EnemyData.AttackPower);
    }

    public override void StateExit()
    {
        fireHilichurl.Agent.updateRotation = true;
        fireHilichurl.TraceAttack = true;
    }

    public override void StateUpDate()
    {
        if (fireHilichurl.TraceAttack)
        {
            fireHilichurl.TraceAttack = false;
            fireHilichurl.Animator.SetTrigger("Attack");
            Weapon.EableSword();
        }

        Vector3 direction = fireHilichurl.PlayerTransform.position - fireHilichurl.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        fireHilichurl.transform.rotation = rotation;
    }
}