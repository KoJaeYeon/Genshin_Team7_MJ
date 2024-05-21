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
    StampAttack,
    DriftAttack
}

public class Wolf : Enemy
{
    private bool enemyCommand = true;
    private bool battleStart = true;
    private bool turn = true;
    private bool coolTime = true;
    private bool jumpBack = true;
    private int phase = 1;
    private float Timer;
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
        bossState.AddState(BossState.Drift, new WolfAttackState_Drift(this));
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
            case BossPattern.DriftAttack:
                bossAttack = new DriftAttack(this);
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
    public int Phase
    {
        get { return phase; }
        set { phase = value; }
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
    public bool BattleStart
    {
        get { return battleStart; }
        set { battleStart = value; }
    }
    public bool OnSkill
    {
        get { return coolTime; }
        set { coolTime = value; }
    }
    public bool JumpBack
    {
        get { return jumpBack; }
        set { jumpBack = value; }
    }

    public IEnumerator SkillCoolTime()
    {
        Debug.Log("쿨타임 진행중");
        yield return new WaitForSeconds(6.0f);

        coolTime = true;
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

    public void OnJumpBack()
    {
        jumpBack = true;
    }
    public void ChangeAttack()
    {
        bossState.ChangeState(BossState.Attack);
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
        m_Wolf.BossAgent.enabled = true;
        m_Wolf.BossAgent.SetDestination(m_Wolf.PlayerTransform.position);
    }

    public override void StateExit()
    {
        m_Wolf.BossAnimator.SetBool("isMove", false);
        m_Wolf.BossAgent.SetDestination(m_Wolf.transform.position);
        m_Wolf.BossAgent.enabled = false;
    }
    public override void StateFixedUpdate()
    {
        if (m_Wolf.BattleStart)
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
    }

    private float Distance()
    {
        return Vector3.Distance(m_Wolf.transform.position, m_Wolf.PlayerTransform.position);
    }
}

public class WolfAttackState : WolfState
{
    public WolfAttackState(Wolf wolf) : base(wolf) { }

    private float meleeDistance = 7.0f;
    public override void StateEnter()
    {
        
    }

    public override void StateExit()
    {
        
    }

    public override void StateFixedUpdate()
    {
        float angle = Angle();
        float distance = Distance();

        //if(distance <= meleeDistance && m_Wolf.OnSkill)
        //{
        //    m_Wolf.State.ChangeState(BossState.Claw);
        //    m_Wolf.OnSkill = false;
        //    m_Wolf.StartCoroutine(m_Wolf.SkillCoolTime());
        //}
        //if (!m_Wolf.OnSkill)
        //    Turn(angle);

        JumpBack(angle, distance);



        //if(m_Wolf.Phase == 1)
        //{
        //    PhaseOneAttack(angle, distance);
        //}
        //else
        //{

        //}
    }

    private void PhaseOneAttack(float Angle, float Distance)
    {
        if (m_Wolf.OnSkill)
        {
            if(Distance <= meleeDistance)
            {
                MeleeAttack(Angle);
            }
            else
            {
                RangedAttack(Distance);
            }
            m_Wolf.OnSkill = false;
            m_Wolf.StartCoroutine(m_Wolf.SkillCoolTime());
        }
        else
            Turn(Angle);
    }

    private void MeleeAttack(float Angle)
    {
        float Abs = Mathf.Abs(Angle);

        if(Abs <= 60)
        {
            m_Wolf.State.ChangeState(BossState.Claw);
        }
        else if(Abs <= 120)
        {
            m_Wolf.State.ChangeState(BossState.Drift);
        }
        else
        {
            m_Wolf.State.ChangeState(BossState.Tail);
        }
    }

    private void RangedAttack(float Distance)
    {
        if(Distance <= 10.0f)
        {
            m_Wolf.State.ChangeState(BossState.Jump);
        }
        else
        {
            m_Wolf.State.ChangeState(BossState.Charge);
        }
    }

    private float Angle()
    {
        Vector3 targetDirection = (m_Wolf.PlayerTransform.position - m_Wolf.transform.position).normalized;
        Vector3 selfDirection = m_Wolf.transform.forward;

        float angle = Vector3.SignedAngle(selfDirection, targetDirection, Vector3.up);

        return angle;
    }

    private float Distance()
    {
        return Vector3.Distance(m_Wolf.transform.position, m_Wolf.PlayerTransform.position);
    }

    private void JumpBack(float Angle, float Distance)
    {
        float Abs = Mathf.Abs(Angle);

        bool Back = Abs < 90 && Distance <= 4.0f && m_Wolf.JumpBack;

        if (Back)
        {
            Vector3 targetDirection = m_Wolf.PlayerTransform.position - m_Wolf.transform.position;

            targetDirection.y = 0;
            
            float angle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;

            m_Wolf.transform.rotation = Quaternion.Euler(0, angle, 0);

            //Quaternion targetRot = Quaternion.Euler(0, Angle, 0);
            //m_Wolf.transform.rotation = Quaternion.RotateTowards(m_Wolf.transform.rotation, targetRot, Time.fixedDeltaTime * 0.5f);

            m_Wolf.BossAnimator.SetTrigger("JumpBack");
            m_Wolf.JumpBack = false;
        }
    }

    private void Turn(float Angle)
    {
        if (Angle >= 90.0f && m_Wolf.Turn)
        {
            m_Wolf.BossAnimator.SetTrigger("TurnRight");
            m_Wolf.Turn = false;
        }
        else if (Angle <= -90.0f && m_Wolf.Turn)
        {
            m_Wolf.BossAnimator.SetTrigger("TurnLeft");
            m_Wolf.Turn = false;
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

        m_Wolf.Attack.BossAttack();
    }
    public override void StateExit() { }
    public override void StateFixedUpdate() { }
}

public class WolfAttackState_Drift : WolfState
{
    public WolfAttackState_Drift(Wolf wolf) : base(wolf) { }
    
    public override void StateEnter()
    {
        m_Wolf.BossPattern = BossPattern.DriftAttack;
        m_Wolf.SetPattern(m_Wolf.BossPattern);

        m_Wolf.Attack.BossAttack();
    }
    public override void StateExit() { }
    public override void StateFixedUpdate() { }
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

