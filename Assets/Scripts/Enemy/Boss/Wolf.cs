using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum BossPattern
{
    JumpAttack,
    TailAttack,
    ClawAttack,
    ChargeAttack
}

public class Wolf : Enemy
{
    private new void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        EnemyHealthDic = new Dictionary<Enemy, float>();
        bossRigid = GetComponent<Rigidbody>();

        bossState = gameObject.AddComponent<BossStateMachine>();
        bossState.AddState(BossState.Idle, new WolfIdle(this));
        bossState.AddState(BossState.Move, new WolfMove(this));
        bossState.AddState(BossState.TailState, new WolfTailState(this));
        bossState.AddState(BossState.ClawState, new WolfClawState(this));
        bossState.AddState(BossState.JumpState, new WolfJumpState(this));
        bossState.AddState(BossState.ChargeState, new WolfChargeState(this));

        enemyData = new EnemyData(1000000f, 50f, 4f, 0.3f, 9999, Element.Ice);
        EnemyHealthDic.Add(this, enemyData.Health);
    }
    
    private IPattern bossAttack;
    private BossPattern Pattern;
    private Rigidbody bossRigid;
    public BossPattern BossPattern
    {
        get { return Pattern; }
        set { Pattern = value; }
    }
    public IPattern Attack => bossAttack;
    public BossStateMachine State => bossState;
    public Animator BossAnimator => animator;
    public NavMeshAgent BossAgent => agent;
    public Transform PlayerTransform => Player;
    public Rigidbody BossRigid => bossRigid;


    public void SetPattern(BossPattern bossPattern)
    {

        Component component = gameObject.GetComponent<IPattern>() as Component;

        if (component != null)
        {
            Destroy(component);
        }

        switch (bossPattern)
        {
            case BossPattern.JumpAttack:
                bossAttack = gameObject.AddComponent<JumpAttack>();
                break;
            case BossPattern.TailAttack:
                bossAttack = gameObject.AddComponent<TailAttack>();
                break;
            case BossPattern.ClawAttack:
                bossAttack = gameObject.AddComponent<ClawAttack>();
                break;
            case BossPattern.ChargeAttack:
                bossAttack = gameObject.AddComponent<ChargeAttack>();
                break;
        }
    }

    public override void Damaged(Enemy enemy, float damage, Element element)
    {
        
    }

    public override void Splash(float damage)
    {
        
    }

    // Animation Event ----------------------------------------------
    public void SetDestination()
    {
        agent.SetDestination(Player.position);
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
    int SelectIdle = 0;
    
    public WolfIdle(Wolf wolf) : base(wolf) { }

    public override void StateEnter()
    {
        SelectIdle = Random.Range(0, 3);
        m_Wolf.BossAnimator.SetFloat("Idle", SelectIdle);
    }

    public override void StateExit()
    {
        SelectIdle = 0;
    }
    public override void StateFixedUpdate()
    {
        if (m_Wolf.BossAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            m_Wolf.State.ChangeState(BossState.Move);
    }
}

public class WolfMove : WolfState
{
    private bool Move;
    public WolfMove(Wolf wolf) : base(wolf) { }
    
    public override void StateEnter()
    {
        Move = true;
        m_Wolf.BossAnimator.SetBool("isMove", Move);
    }

    public override void StateExit()
    {
        
    }

    public override void StateFixedUpdate()
    {
     //   m_Wolf.BossAnimator.SetFloat("MovePosY", m_Wolf.BossRigid.angularVelocity.y);
        
    }
}

public class WolfJumpState : WolfState
{
    public WolfJumpState(Wolf wolf) : base(wolf) { }

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

public class WolfTailState : WolfState
{
    public WolfTailState(Wolf wolf) : base(wolf) { }

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

public class WolfClawState : WolfState
{
    public WolfClawState(Wolf wolf) : base(wolf) { }
    
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

public class WolfChargeState : WolfState
{
    public WolfChargeState(Wolf wolf) : base(wolf) { }
    
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

