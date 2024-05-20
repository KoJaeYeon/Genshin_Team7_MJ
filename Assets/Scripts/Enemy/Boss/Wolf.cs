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
    private BossPattern Pattern;
    private Rigidbody bossRigid;

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
        bossState.AddState(BossState.Move, new WolfMove(this));
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

    public override void Damaged(Enemy enemy, float damage, Element element)
    {
        
    }

    public override void Splash(float damage)
    {
        
    }

    // Animation Event ----------------------------------------------
    public void SetAttack() // 1Phase -------------------------------
    {
        int random = Random.Range(0, 4);

        switch (random)
        {
            case (int)BossPattern.JumpAttack:
                bossState.ChangeState(BossState.Jump);
                break;
            case (int)BossPattern.TailAttack:
                bossState.ChangeState(BossState.Tail);
                break;
            case (int)BossPattern.ClawAttack:
                bossState.ChangeState(BossState.Claw);
                break;
            case (int)BossPattern.ChargeAttack:
                bossState.ChangeState(BossState.Charge);
                break;
        }
    }

    public void Rotation()
    {

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

public class WolfMove : WolfState
{
    public WolfMove(Wolf wolf) : base(wolf) { }

    float timer = 0;

    public override void StateEnter()
    {
        m_Wolf.BossAnimator.SetBool("isMove", true);
    }

    public override void StateExit()
    {
        m_Wolf.BossAnimator.SetBool("isMove", false);
        timer = 0;
    }

    public override void StateFixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        m_Wolf.BossAgent.SetDestination(m_Wolf.PlayerTransform.position);

        if(timer >= 5.0f)
        {
            m_Wolf.State.ChangeState(BossState.Idle);
        }
        else
        {
            if (Vector3.Distance(m_Wolf.transform.position, m_Wolf.PlayerTransform.position) <= m_Wolf.BossAgent.stoppingDistance)
            {
                m_Wolf.State.ChangeState(BossState.Idle);
            }
        }
    }
}


public class WolfIdle : WolfState
{
    public WolfIdle(Wolf wolf) : base(wolf) { }

    public override void StateEnter()
    {
        m_Wolf.BossAgent.SetDestination(m_Wolf.transform.position);
    }

    public override void StateExit()
    {
    
    }
    public override void StateFixedUpdate()
    {
        
        //m_Wolf.BossAgent.SetDestination(m_Wolf.PlayerTransform.position);
        //AnimatorStateInfo StateInfo = m_Wolf.BossAnimator.GetCurrentAnimatorStateInfo(0);

        //if(StateInfo.IsName("Idle") && StateInfo.normalizedTime >= 1.0f)
        //{
        //    //if(Distance() > 6.0f)
        //    //{
        //    //    m_Wolf.State.ChangeState(BossState.Jump);
        //    //}
        //    //else
        //    //{
        //    //    RandomPattern();
        //    //}
        //    m_Wolf.BossAnimator.SetTrigger("TurnLeft");
            

        //}
    }

    private void RandomPattern()
    {
        //int randomPattern = Random.Range(0, 3);
        int randomPattern = 2;
        switch (randomPattern)
        {
            case 0:
                m_Wolf.State.ChangeState(BossState.Stamp);
                break;
            case 1:
                m_Wolf.State.ChangeState(BossState.Tail);
                break;
            case 2:
                m_Wolf.State.ChangeState(BossState.Claw);
                break;
        }
    }



    private float Distance()
    {
        return Vector3.Distance(m_Wolf.transform.position, m_Wolf.PlayerTransform.position);
    }

    private bool Left()
    {
        Vector3 targetPos = m_Wolf.PlayerTransform.position - m_Wolf.transform.position;
        float Dot = Vector3.Dot(m_Wolf.transform.right, targetPos);

        return Dot < 0;
    }
    private bool Right()
    {
        return !Left();
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

