using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum AndriusPattern
{
    Idle,
    Move,
    Attack,
    Jump,
    Claw,
    Charge,
    Stamp,
    Drift,
    Howl
}

public class Andrius : Enemy, IColor, IAndriusClawEvent
{
    [Header("EffectPool")]
    public GameObject effectPool;
    [Header("AndriusSlider")]
    public Slider[] BossSlider;

    private bool battleStart = true;
    private bool turn = true;
    private bool moveStop = false;
    private bool jumpBack = true;
    private bool isJump = true;
    private bool isCharge = true;
    private bool isRunStop = false;
    private float paralyzation;

    private AndriusPattern _andriusPattern;
    private IPattern _currentPattern;

    private Color BossColor = Color.blue;

    private Slider PaSlider;
    private Rigidbody bossRigid;
    private GameObject Pa;
    private Dictionary<AndriusPattern, IPattern> _patternDic;
    private Action _leftClawEvent;
    private Action _rightClawEvent;
    
    private new void Awake()
    {
        InitializeAndriusComponent();
        InitializeAndriusData();
        InitializeState();
    }

    private void InitializeAndriusComponent()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        bossRigid = GetComponent<Rigidbody>();
        HpSlider = BossSlider[0].GetComponent<Slider>();
        PaSlider = BossSlider[1].GetComponent<Slider>();
        EnemyHealthDic = new Dictionary<Enemy, float>();
        _patternDic = new Dictionary<AndriusPattern, IPattern>();
        AndriusEventManager.Instance.RegisterClawEvent(this);
        AddPattern();
    }

    private void InitializeAndriusData()
    {
        enemyData = new EnemyData(2000f, 1000f, 4f, 0.5f, 9999, Element.Ice);
        EnemyHealthDic.Add(this, enemyData.Health);
        paralyzation = 100f;
        Hp = HpSlider.fillRect.transform.parent.gameObject;
        Pa = PaSlider.fillRect.transform.parent.gameObject;
    }
    
    public void InitializeState()
    {
        bossState = gameObject.AddComponent<BossStateMachine>();
        bossState.AddState(BossState.Idle, new AndriusIdleState(this));
        bossState.AddState(BossState.Move, new AndriusWalkState(this));
        bossState.AddState(BossState.Attack, new AndriusAttackState(this));
        bossState.AddState(BossState.Howl, new Andrius_Howl(this));
        bossState.AddState(BossState.Stamp, new Andrius_Stamp(this));
        bossState.AddState(BossState.Jump, new Andrius_Jump(this));
        bossState.AddState(BossState.Claw, new Andrius_Claw(this));
        bossState.AddState(BossState.Drift, new Andrius_Drift(this));
        bossState.AddState(BossState.Charge, new Andrius_Charge(this));
    }

    public void SetPattern(AndriusPattern newPattern)
    {
        switch (newPattern)
        {
            case AndriusPattern.Idle:
                _currentPattern = GetPattern(newPattern);
                break;
            case AndriusPattern.Move:
                _currentPattern = GetPattern(newPattern);
                break;
            case AndriusPattern.Attack:
                _currentPattern = GetPattern(newPattern);
                break;
            case AndriusPattern.Jump:
                _currentPattern = GetPattern(newPattern);
                break;
            case AndriusPattern.Claw:
                _currentPattern = GetPattern(newPattern);
                break;
            case AndriusPattern.Charge:
                _currentPattern = GetPattern(newPattern);
                break;
            case AndriusPattern.Stamp:
                _currentPattern = GetPattern(newPattern);
                break;
            case AndriusPattern.Drift:
                _currentPattern = GetPattern(newPattern);
                break;
            case AndriusPattern.Howl:
                _currentPattern = GetPattern(newPattern);
                break;
        }
    }

    private void AddPattern()
    {
        _patternDic[AndriusPattern.Idle] = new AndriusParalyzation();
        _patternDic[AndriusPattern.Move] = new AndriusWalk();
        _patternDic[AndriusPattern.Attack] = new AndriusAttack();
        _patternDic[AndriusPattern.Jump] = new JumpAttack();
        _patternDic[AndriusPattern.Claw] = new ClawAttack();
        _patternDic[AndriusPattern.Charge] = new ChargeAttack();
        _patternDic[AndriusPattern.Stamp] = new StampAttack();
        _patternDic[AndriusPattern.Drift] = new DriftAttack();
        _patternDic[AndriusPattern.Howl] = new HowlAttack();
    }

    private IPattern GetPattern(AndriusPattern currentPattern)
    {
        if(_patternDic.TryGetValue(currentPattern, out IPattern pattern))
        {
            return pattern;
        }

        return null;
    }

    public IPattern CurrentPattern => _currentPattern;
    public BossStateMachine State => bossState;
    public Animator BossAnimator => animator;
    public Transform PlayerTransform => Player;

    public AndriusPattern Pattern
    {
        get { return Pattern; }
        set { Pattern = value; }
    }
    public float Paralyzation
    {
        get { return paralyzation; }
        set { paralyzation = value; }
    }
    public bool Turn
    {
        get { return turn; }
        set { turn = value; }
    }
    
    public bool MoveStop
    {
        get { return moveStop; }
        set { moveStop = value; }
    }
    public bool JumpBack
    {
        get { return jumpBack; }
        set { jumpBack = value; }
    }
  
    public bool IsRunStop
    {
        get { return isRunStop; }
        set { isRunStop = value; }
    }
 
    protected override void DropItem(Enemy enemy)
    {
        DropObject dropObject = PoolManager.Instance.Get_DropObject(UnityEngine.Random.Range(1007, 1010));
        dropObject.gameObject.transform.position = transform.position + Vector3.up*1.5f;
    }

    public override void TakeDamage(float damage, Element element, Character attacker)
    {
        EnemyHealthDic[this] -= CalculateDamage(damage, element);
        paralyzation -= 10f;

        if (HpSlider != null)
        {
            HpSlider.value = EnemyHealthDic[this];
        }
        if (PaSlider != null)
        {
            PaSlider.value = paralyzation;
        }

        animator.SetTrigger("Hit");
        PoolManager.Instance.Get_Text(damage, transform.position, element);

        if (EnemyHealthDic[this] <= 0)
        {
            Hp.SetActive(false);
            Pa.SetActive(false);
            StartCoroutine(Die(this, attacker));
        }
    }

    protected override IEnumerator Die(Enemy enemy, Character attacker)
    {
        enemy.gameObject.layer = (int)EnemyLayer.isDead;
        this.animator.SetTrigger("Die");
        DropElement(enemy);
        DropItem(enemy);

        if (attacker != null)
        {
            attacker.OnEnemyKilled();
        }

        yield return new WaitForSeconds(1.5f);
        effectPool.SetActive(false);
        enemy.gameObject.SetActive(false);
    }

    public Color GetColor()
    {
        return BossColor;
    }
    
    public float GetAtk()
    {
        return enemyData.AttackPower;
    }
    public void LeftClawEvent(Action callBack)
    {
        _leftClawEvent += callBack;
    }

    public void RightClawEvent(Action callBack)
    {
        _rightClawEvent += callBack;
    }

    // Animation Event ----------------------------------------------
    public void OnTurn()
    {
        turn = true;
        moveStop = false;
    }
    public void Stop()
    {
        moveStop = true;
    }

    public void OnJumpBack()
    {
        jumpBack = true;
        moveStop = false;
    }
    public void ChangeAttack()
    {
        bossState.ChangeState(BossState.Attack);
    }    

    public void RunStop()
    {
        isRunStop = false;
    }

    public void LeftClaw()
    {
        _leftClawEvent?.Invoke();
    }

    public void RightClaw()
    {
        _rightClawEvent?.Invoke();
    }
}