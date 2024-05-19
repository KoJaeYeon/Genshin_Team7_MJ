using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEditor.Rendering;
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
        bossState.AddState(BossState.Ready, new WolfReady(this));
        bossState.AddState(BossState.Idle, new WolfIdle(this));
        bossState.AddState(BossState.Battle, new WolfBattle(this));
        bossState.AddState(BossState.TailState, new WolfTailState(this));
        bossState.AddState(BossState.ClawState, new WolfClawState(this));
        bossState.AddState(BossState.JumpState, new WolfJumpState(this));
        bossState.AddState(BossState.ChargeState, new WolfChargeState(this));

        enemyData = new EnemyData(1000000f, 50f, 4f, 0.3f, 9999, Element.Ice);
        EnemyHealthDic.Add(this, enemyData.Health);
    }

    private bool start = true;
    private bool enemyCommand = false;
    private IPattern bossAttack;
    private BossPattern Pattern;
    private Rigidbody bossRigid;

    public BossPattern BossPattern
    {
        get { return Pattern; }
        set { Pattern = value; }
    }
    public bool Start
    {
        get { return  start; }
        set { start = value; }
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
    public void MoveAnimation()
    {
        if(Vector3.Distance(transform.position, Player.position) < 40.0f)
        {
            animator.SetBool("isReady", false);
            animator.SetBool("isMove", true);
        }
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

public class WolfReady : WolfState
{
    public WolfReady(Wolf wolf) : base(wolf) { }

    private float StopDistance = 10.0f;
    
    public override void StateEnter()
    {
        m_Wolf.BossAnimator.SetBool("isReady", true);
    }
    public override void StateExit()
    {
        m_Wolf.BossAnimator.SetBool("isMove", false);
    }

    public override void StateFixedUpdate()
    {
        float currentDistance = Distance();

        if(currentDistance <= StopDistance)
        {
            m_Wolf.State.ChangeState(BossState.Idle);
        }
    }
    private float Distance()
    {
        return Vector3.Distance(m_Wolf.transform.position, m_Wolf.PlayerTransform.position);
    }
}

public class WolfIdle : WolfState
{
    public WolfIdle(Wolf wolf) : base(wolf) { }

    private int SelectIdle;

    public override void StateEnter()
    {
        SelectIdle = Random.Range(0, 3);
        m_Wolf.BossAnimator.SetFloat("Idle", SelectIdle);
    }

    public override void StateExit()
    {
        
    }
    public override void StateFixedUpdate()
    {
        


    }

    
}

public class WolfBattle : WolfState
{

    public WolfBattle(Wolf wolf) : base(wolf) { }
    
    public override void StateEnter()
    {
        
    }

    public override void StateExit()
    {
        
    }

    public override void StateFixedUpdate()
    {
       
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

