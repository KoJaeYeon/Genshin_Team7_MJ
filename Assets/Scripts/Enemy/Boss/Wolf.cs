using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public enum BossPattern
{
    JumpAttack,
    TailAttack,
    ClawAttack,
    ChargeAttack,
    StampAttack
}

public class Wolf : Enemy
{
    private bool enemyCommand = true;
    private bool turn = true;
    private BossPattern Pattern;
    private Rigidbody bossRigid;
    private Vector3 currentTrans;
    

    //Pattern----------------------------------------------------------
    private IPattern bossAttack;
    
    private new void Awake()
    {
        InitWolf();
        InitState();
        SetPattern(BossPattern.JumpAttack);
    }

    
    public void InitWolf()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        bossRigid = GetComponent<Rigidbody>();
        EnemyHealthDic = new Dictionary<Enemy, float>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyData = new EnemyData(1000000f, 50f, 4f, 0.3f, 9999, Element.Ice);
        EnemyHealthDic.Add(this, enemyData.Health);
    }

    public void InitState()
    {
        bossState = gameObject.AddComponent<BossStateMachine>();
        bossState.AddState(BossState.Idle, new WolfIdle(this));
        bossState.AddState(BossState.Attack, new WolfAttackState(this));
        bossState.AddState(BossState.Tail, new WolfAttackState_Tail(this));
        bossState.AddState(BossState.Claw, new WolfAttackState_Claw(this));
        bossState.AddState(BossState.Jump, new WolfAttackState_Jump(this));
        bossState.AddState(BossState.Charge, new WolfAttackState_Charge(this));
        bossState.AddState(BossState.Stamp, new WolfAttackState_Stamp(this));
    }

    public void SetPattern(BossPattern bossPattern)
    {
        switch (bossPattern)
        {
            case BossPattern.JumpAttack:
                bossAttack = new JumpAttack(this);
                break;
            case BossPattern.TailAttack:
                bossAttack = new TailAttack(this);
                break;
            case BossPattern.ClawAttack:
                bossAttack = new ClawAttack(this);
                break;
            case BossPattern.ChargeAttack:
                bossAttack = new ChargeAttack(this);
                break;
            case BossPattern.StampAttack:
                bossAttack = new StampAttack(this);
                break;
        }
    }

    public BossPattern BossPattern
    {
        get { return Pattern; }
        set { Pattern = value; }
    }
    public bool EnemyCommand
    {
        get { return enemyCommand; }
        set { enemyCommand = value; }
    }
    public IPattern Attack => bossAttack;
    public BossStateMachine State => bossState;
    public Animator BossAnimator => animator;
    public NavMeshAgent BossAgent => agent;
    public Transform PlayerTransform => Player;
    public Rigidbody BossRigid => bossRigid;
    public Vector3 CurrentTrans => currentTrans;
    public bool Turn
    {
        get { return turn; }
        set { turn = value; }
    }

    public override void Damaged(Enemy enemy, float damage, Element element)
    {
        
    }

    public override void Splash(float damage)
    {
        
    }

    // Animation Event ----------------------------------------------
    
    public void CurrentTransform()
    {
        currentTrans = transform.position;
    }

    public void OnTurn()
    {
        turn = true;
    }
}

public abstract class WolfState : BossBaseState
{
    protected Wolf m_Wolf;
    public WolfState(Wolf wolf)
    {
        m_Wolf = wolf;
    }
}

public class WolfIdle : WolfState
{
    public WolfIdle(Wolf wolf) : base(wolf) { }

    float timer = 0;

    public override void StateEnter()
    {
        m_Wolf.BossAgent.SetDestination(m_Wolf.PlayerTransform.position);
    }

    public override void StateExit()
    {
        m_Wolf.BossAnimator.SetBool("isMove", false);
        m_Wolf.BossAgent.SetDestination(m_Wolf.transform.position);
    }
    public override void StateFixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (Distance() > m_Wolf.BossAgent.stoppingDistance && timer <= 7.0f)
        {
            m_Wolf.BossAgent.SetDestination(m_Wolf.PlayerTransform.position);
            m_Wolf.BossAnimator.SetBool("isMove", true);
        }
        else
            m_Wolf.State.ChangeState(BossState.Attack);
    }

    private float Distance()
    {
        return Vector3.Distance(m_Wolf.transform.position, m_Wolf.PlayerTransform.position);
    }
}

public class WolfAttackState : WolfState
{
    public WolfAttackState(Wolf wolf) : base(wolf) { }

  

    public override void StateEnter()
    {
        m_Wolf.BossAgent.enabled = false;
    }

    public override void StateExit()
    {
        
    }

    public override void StateFixedUpdate()
    {
        Angle();
    }
    
    //private void Rot()
    //{
    //    m_Wolf.BossAnimator.SetTrigger("TurnLeft");

    //    Vector3 dir = (m_Wolf.PlayerTransform.position - m_Wolf.transform.position).normalized;

    //    Quaternion targetRot = Quaternion.LookRotation(dir);

    //    m_Wolf.transform.rotation = Quaternion.Slerp(m_Wolf.transform.rotation, targetRot, 1.0f * Time.fixedDeltaTime);
    //}

    private void Angle()
    {
        Vector3 targetDirection = (m_Wolf.PlayerTransform.position - m_Wolf.transform.position).normalized;
        Vector3 selfDirection = m_Wolf.transform.forward;

        float angle = Vector3.SignedAngle(selfDirection, targetDirection, Vector3.up);

        if (angle >= 120.0f && m_Wolf.Turn)
            m_Wolf.BossAnimator.SetTrigger("TurnRight");
        else if (angle <= -120.0f && m_Wolf.Turn)
            m_Wolf.BossAnimator.SetTrigger("TurnLeft");
    }
    private void Rot()
    {
        Vector3 targetPos = m_Wolf.PlayerTransform.position - m_Wolf.transform.position;
        float Dot = Vector3.Dot(m_Wolf.transform.right, targetPos);

        if(Dot < 0)
        {
            Quaternion targetRot = Quaternion.LookRotation(-targetPos, Vector3.up);
            m_Wolf.transform.rotation = Quaternion.Slerp(m_Wolf.transform.rotation, targetRot, 1.0f * Time.fixedDeltaTime);
        }
        else
        {
            Quaternion targetRot = Quaternion.LookRotation(targetPos, Vector3.up);
            m_Wolf.transform.rotation = Quaternion.Slerp(m_Wolf.transform.rotation, targetRot, 1.0f * Time.fixedDeltaTime);
        }
        
    }
    
}

public class WolfAttackState_Stamp : WolfState
{
    public WolfAttackState_Stamp(Wolf wolf) : base(wolf) { }
    
    public override void StateEnter()
    {
        m_Wolf.BossPattern = BossPattern.StampAttack;
        m_Wolf.SetPattern(m_Wolf.BossPattern);
    }

    public override void StateExit()
    {
        
    }

    public override void StateFixedUpdate()
    {
        m_Wolf.Attack.BossAttack();
    }
}

public class WolfAttackState_Jump : WolfState
{
    public WolfAttackState_Jump(Wolf wolf) : base(wolf) { }

    public override void StateEnter()
    {
        m_Wolf.BossPattern = BossPattern.JumpAttack;
        m_Wolf.SetPattern(m_Wolf.BossPattern);
        
    }

    public override void StateExit()
    {
        
    }

    public override void StateFixedUpdate()
    {
        m_Wolf.Attack.BossAttack();
    }
}

public class WolfAttackState_Tail : WolfState
{
    public WolfAttackState_Tail(Wolf wolf) : base(wolf) { }

    public override void StateEnter()
    {
        m_Wolf.BossPattern = BossPattern.TailAttack;
        m_Wolf.SetPattern(m_Wolf.BossPattern);
    }

    public override void StateExit()
    {
        
    }

    public override void StateFixedUpdate()
    {
        m_Wolf.Attack.BossAttack();
    }
}

public class WolfAttackState_Claw : WolfState
{
    public WolfAttackState_Claw(Wolf wolf) : base(wolf) { }
    
    public override void StateEnter()
    {
        m_Wolf.BossPattern = BossPattern.ClawAttack;
        m_Wolf.SetPattern(m_Wolf.BossPattern);
    }

    public override void StateExit()
    {
        
    }

    public override void StateFixedUpdate()
    {
        m_Wolf.Attack.BossAttack();
    }
}

public class WolfAttackState_Charge : WolfState
{
    public WolfAttackState_Charge(Wolf wolf) : base(wolf) { }
    
    public override void StateEnter()
    {
        m_Wolf.BossPattern = BossPattern.ChargeAttack;
        m_Wolf.SetPattern(m_Wolf.BossPattern);
    }

    public override void StateExit()
    {
     
    }

    public override void StateFixedUpdate()
    {
        m_Wolf.Attack.BossAttack();
    }
}

