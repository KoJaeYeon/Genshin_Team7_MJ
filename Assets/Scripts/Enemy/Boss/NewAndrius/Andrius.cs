using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;

public class Andrius : Enemy, IColor, IAndriusClawEvent
{
    [Header("EffectPool")]
    public GameObject effectPool;

    [Header("AndriusSlider")]
    public Slider[] BossSlider;

    [Header("WalkPos")]
    [SerializeField] private GameObject[] _walkPos;

    private List<Transform> _selectPositionList;    
    private Slider PaSlider;
    private GameObject Pa;
    private Action _leftClawEvent;
    private Action _rightClawEvent;
    private Color BossColor;

    private new void Awake()
    {
        EnemyHealthDic = new Dictionary<Enemy, float>();
    }

    private void OnEnable()
    {
        AndriusEventManager.Instance.RegisterClawEvent(this);
    }

    private void Start()
    {
        GetData();
        GetWalkPosition();
        InitializeAndriusComponent();
        InitializeState();
        InitializeAndriusValue();
    }

    private void GetData()
    {
        _baseData = EnemyCSVLoader.Instance.GetEnemyCSVData<EnemyBaseData>("B105");
        _elementData = EnemyCSVLoader.Instance.GetEnemyCSVData<EnemyElementData>("E105");
        _traceData = EnemyCSVLoader.Instance.GetEnemyCSVData<EnemyTraceData>("T105");
    }

    private void InitializeAndriusValue()
    {
        EnemyHealthDic.Add(this, _baseData.Health);
        AndriusParalyzationData paralyzationData = EnemyCSVLoader.Instance.GetAndriusCSVData<AndriusParalyzationData>("AndriusParalyzationData");
        Paralyzation = paralyzationData.ParalyzationValue;
        agent.stoppingDistance = _traceData.AgentStopDistance;
        agent.speed = _baseData.Speed;
        Hp = HpSlider.fillRect.transform.parent.gameObject;
        Pa = PaSlider.fillRect.transform.parent.gameObject;
        BossColor = _elementData.Color;
    }

    private void InitializeAndriusComponent()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        HpSlider = BossSlider[0].GetComponent<Slider>();
        PaSlider = BossSlider[1].GetComponent<Slider>();
        
    }
    
    public void InitializeState()
    {
        bossState = gameObject.AddComponent<BossStateMachine>();
        bossState.AddState(BossState.Idle, new Andrius_Idle(this));
        bossState.AddState(BossState.Move, new Andrius_Walk(this));
        bossState.AddState(BossState.Howl, new Andrius_Howl(this));
        bossState.AddState(BossState.Stamp, new Andrius_Stamp(this));
        bossState.AddState(BossState.Jump, new Andrius_Jump(this));
        bossState.AddState(BossState.Claw, new Andrius_Claw(this));
        bossState.AddState(BossState.Drift, new Andrius_Drift(this));
        bossState.AddState(BossState.Charge, new Andrius_Charge(this));
        bossState.AddState(BossState.Turn, new Andrius_Turn(this));
        bossState.AddState(BossState.Back, new Andrius_Back(this));
    }

    private void GetWalkPosition()
    {
        int randomWalkPos = UnityEngine.Random.Range(0, _walkPos.Length);

        GameObject selectPosObject = _walkPos[randomWalkPos];

        _selectPositionList = new List<Transform>();

        foreach (Transform transform in selectPosObject.transform)
        {
            _selectPositionList.Add(transform);
        }
    }

    public bool IsAction { get; set; } = false;
    public float Paralyzation { get; set; }
    public Transform PlayerTransform => Player;
    public List<Transform> WalkList => _selectPositionList;
    
    protected override void DropItem(Enemy enemy)
    {
        DropObject dropObject = PoolManager.Instance.Get_DropObject(UnityEngine.Random.Range(1007, 1010));
        dropObject.gameObject.transform.position = transform.position + Vector3.up*1.5f;
    }

    public override void TakeDamage(float damage, Element element, Character attacker)
    {
        EnemyHealthDic[this] -= CalculateDamage(damage, element);
        Paralyzation -= 10f;

        if (HpSlider != null)
        {
            HpSlider.value = EnemyHealthDic[this];
        }
        if (PaSlider != null)
        {
            PaSlider.value = Paralyzation;
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
        return _baseData.Power;
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
    public void LeftClaw()
    {
        _leftClawEvent?.Invoke();
    }

    public void RightClaw()
    {
        _rightClawEvent?.Invoke();
    }

    public void ActionReset()
    {
        IsAction = false;
    }


    private void OnDrawGizmos()
    {
        Vector3 forward = transform.forward;

        // 양옆 60도씩 회전한 벡터 계산
        Vector3 leftDirection = Quaternion.Euler(0, -60, 0) * forward;
        Vector3 rightDirection = Quaternion.Euler(0, 60, 0) * forward;

        // 몬스터 위치
        Vector3 position = transform.position;

        // 기즈모 색 설정
        Gizmos.color = Color.cyan;
        // 전방 벡터
        Gizmos.DrawLine(position, position + forward * 20);

        Gizmos.color = Color.red;

        // 왼쪽 60도 벡터
        Gizmos.DrawLine(position, position + leftDirection * 8);

        // 오른쪽 60도 벡터
        Gizmos.DrawLine(position, position + rightDirection * 8);

        Gizmos.color = Color.yellow;

        Vector3 leftDirection2 = Quaternion.Euler(0, -61, 0) * forward;
        Vector3 rightDirection2 = Quaternion.Euler(0, 61, 0) * forward;

        Vector3 leftDirection4 = Quaternion.Euler(0, -119, 0) * forward;
        Vector3 rightDirection4 = Quaternion.Euler(0, 119, 0) * forward;

        Gizmos.DrawLine(position, position + leftDirection2 * 8);
        Gizmos.DrawLine(position, position + rightDirection2 * 8);

        Gizmos.DrawLine(position, position + leftDirection4 * 8);
        Gizmos.DrawLine(position, position + rightDirection4 * 8);

        Gizmos.color = Color.blue;

        Vector3 leftDirection3 = Quaternion.Euler(0, -120, 0) * forward;
        Vector3 rightDirection3 = Quaternion.Euler(0, 120, 0) * forward;

        Gizmos.DrawLine(position, position + leftDirection3 * 8);
        Gizmos.DrawLine(position, position + rightDirection3 * 8);
    }
}