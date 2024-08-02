using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.AI;
using UnityEngine.UI;


public enum EnemyLayer
{
    isAlive = 3,
    isDead = 6
}

public enum Element
{
    Normal,
    Fire,
    Ice,
    Lightning,
    Water
}
public class Enemy : MonoBehaviour
{
    //Node
    protected Node _node;

    //Patrol
    protected List<Transform> _wayPointList;
    protected List<Transform> _useWayPointList;
    protected Transform _currentTransform;
    protected float _waitTime;
    protected float _waitCount;
    protected bool isWaiting;

    //Trace
    protected bool isTracking;

    //Player
    protected GameObject _player;
    protected float _radius;
    protected int _playerLayer;

    //NavMeshAgent
    protected NavMeshAgent _agent;

    //Data
    protected EnemyData _data;
    protected Dictionary<Enemy, float> _enemyHealthDic;

    //Slider
    protected Slider _hpSlider;
    protected GameObject _sliderObject;

    //Animator
    protected Animator _animator;

    //SkinnedMeshRenderer
    protected SkinnedMeshRenderer _skinnedMeshRenderer;

    //Hit
    protected Element _enemyElement;
    private IColor _enemyColor;
    private Color _elementColor;
    private int _elementCount;
    //---------------------------------------------------------------

    protected EnemyStateMachine state;
    protected BossStateMachine bossState;
    protected MonsterWeapon Weapon;
    protected Animator animator;
    protected Transform Player;
    protected NavMeshAgent agent;
    protected Dictionary<Enemy, float> EnemyHealthDic;
    protected GameObject Hp;
    protected Slider HpSlider;
    protected SkinnedMeshRenderer EnemyMesh;
    
    private IColor color;
    private Color ElementColor;
    protected Element HitElement;
    protected EnemyData enemyData;
    protected float traceDistance = 5.0f;
    protected bool traceMove = true;
    protected bool attack = true;
    protected bool isHitMotion = true;
    private int elementCount = 5;

    protected virtual void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Weapon = transform.GetComponentInChildren<MonsterWeapon>(); 
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        EnemyMesh = transform.GetComponentInChildren<SkinnedMeshRenderer>();
        HpSlider = transform.GetComponentInChildren<Slider>();
        Hp = HpSlider.gameObject;

        EnemyMesh.material = this.EnemyMesh.materials[0];
        EnemyHealthDic = new Dictionary<Enemy, float>();


        InitializeEnemy();
    }

    public void InitializeEnemy()
    {
        _waitTime = 4f;
        isWaiting = true;
        _radius = 6f;
        _playerLayer = LayerMask.GetMask("Player");
        isTracking = false;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _skinnedMeshRenderer = transform.GetComponentInChildren<SkinnedMeshRenderer>();
        _hpSlider = transform.GetComponentInChildren<Slider>();
        _sliderObject = _hpSlider.gameObject;
        _skinnedMeshRenderer.material = this._skinnedMeshRenderer.materials[0];
        _enemyHealthDic = new Dictionary<Enemy, float>();
    }

    public virtual INode.NodeState Patrol()
    {
        return INode.NodeState.Fail;
    }

    public virtual INode.NodeState CheckPlayer()
    {
        return INode.NodeState.Fail;
    }

    public virtual INode.NodeState TrackingPlayer()
    {
        return INode.NodeState.Fail;
    }

    public virtual INode.NodeState IsTracking()
    {
        return INode.NodeState.Fail;
    }

    public virtual INode.NodeState CheckAttackRange()
    {
        return INode.NodeState.Fail;
    }

    public virtual INode.NodeState EnemyAttack()
    {
        return INode.NodeState.Fail;
    }

    public void SetDestination(Vector3 position, float speed, float stoppingDistance, bool isMoving)
    {
        _animator.SetBool("Move", isMoving);
        _animator.SetBool("Trace", !isMoving);
        _agent.stoppingDistance = stoppingDistance;
        _agent.speed = speed;
        _agent.SetDestination(position);    
    }

    public Transform FindWayPoint(List<Transform> useWayList, List<Transform> wayPointList, ref Transform currentTransform)
    {
        if(useWayList == null || useWayList.Count == 0)
        {
            useWayList = new List<Transform>(wayPointList);
            if(currentTransform != null)
            {
                useWayList.Remove(currentTransform);
            }
        }

        Transform newRandomTransform = useWayList[Random.Range(0, useWayList.Count)];
        useWayList.Remove(newRandomTransform);
        currentTransform = newRandomTransform;
        return newRandomTransform;
    }

    public virtual void Splash(float damage) { }

    public virtual void TakeDamage(float damage, Element element, Character attacker)
    {
        EnemyHealthDic[this] -= CalculateDamage(damage, element);
        HpSlider.value = EnemyHealthDic[this];
        transform.LookAt(Player.position);
        animator.SetTrigger("Hit");

        PoolManager.Instance.Get_Text(damage, transform.position , element);

        if (EnemyHealthDic[this] <= 0)
        {
            Hp.SetActive(false);
            StartCoroutine(Die(this, attacker));
        }
        else
        {
            if (element != Element.Normal)
            {
                HitDropElement(element);
            }
            if (isHitMotion)
            {
                StartCoroutine(HitMotion(this));
            }

        }
            
    }

    protected float CalculateDamage(float damage, Element element) 
    {
        switch (element)
        {
            case Element.Fire:
                if(enemyData.element == Element.Ice)
                {
                    Debug.Log("융해");
                    damage *= 2f;
                }
                else if(enemyData.element == Element.Lightning)
                {
                    Debug.Log("과부화");
                    damage -= damage * enemyData.Defence;
                    SplashAttack();
                }
                else
                {
                    damage -= damage * enemyData.Defence;
                }
                break;
            case Element.Ice:
                if(enemyData.element == Element.Fire)
                {
                    Debug.Log("융해");
                    damage *= 1.5f;
                }
                else if(enemyData.element == Element.Lightning)
                {
                    Debug.Log("초전도");
                    damage -= damage * enemyData.Defence;
                    SplashAttack();
                }
                else
                {
                    damage -= damage * enemyData.Defence;
                }
                break;
            case Element.Lightning:
                if(enemyData.element == Element.Fire)
                {
                    Debug.Log("과부화");
                    damage -= damage * enemyData.Defence;
                    SplashAttack();
                }
                else if(enemyData.element == Element.Ice)
                {
                    Debug.Log("초전도");
                    damage -= damage * enemyData.Defence;
                    SplashAttack();
                }
                else
                {
                    damage -= damage * enemyData.Defence;
                }
                break;
            case Element.Normal:
                damage -= damage * enemyData.Defence;
                break;
        }
        return damage;
    }

    private void SplashAttack()
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, 2.0f, LayerMask.GetMask("Monster"));

        for(int i = 0; i < collider.Length; i++)
        {
            Enemy checkEnemy = collider[i].GetComponent<Enemy>();

            if (checkEnemy != null)
            {
                checkEnemy.Splash(5f);
            }
        }
    }

    protected void DropElement(Enemy enemy)
    {
        if (enemy == null)
            return;

        for(int i = 0; i < elementCount; i++) //나중에 풀매니저에서 끌어오는 코드로 변경해야함!!
        {
            GameObject dropElement = PoolManager.Instance.GetElementObject();
            ElementObject elementObject = dropElement.GetComponent<ElementObject>();
            elementObject.SetColor(EnemyGetColor(enemy));
            dropElement.transform.position = enemy.transform.position;
            dropElement.SetActive(true);
            StartCoroutine(elementObject.UP());
            StartCoroutine(elementObject.DisableObject());
        }
    }

    public void HitDropElement(Element element)
    {
        switch (element)
        {
            case Element.Fire:
                ElementColor = Color.red;
                break;
            case Element.Ice:
                ElementColor = Color.blue;
                break;
            case Element.Lightning:
                ElementColor = Color.yellow;
                break;
            case Element.Normal:
                ElementColor = Color.white;
                break;
            default:
                ElementColor = Color.white;
                break;
            }

        for(int i = 0; i < elementCount; i++)
        {
            GameObject hitElement = PoolManager.Instance.GetElementObject();
            ElementObject elementObject = hitElement.GetComponent<ElementObject>();
            elementObject.SetColor(ElementColor);
            hitElement.transform.position = transform.position;
            hitElement.SetActive(true);
            StartCoroutine(elementObject.UP());
            StartCoroutine(elementObject.DisableObject());
        }
    }

    public Color EnemyGetColor(Enemy enemy)
    {
        color = enemy.GetComponent<IColor>();
        ElementColor = color.GetColor();
        return ElementColor;
    }

    protected virtual void DropItem(Enemy enemy)
    {
        DropObject dropObject = PoolManager.Instance.Get_DropObject(Random.Range(1001, 1007));
        dropObject.gameObject.transform.position = transform.position + Vector3.up*1.5f;

        //경험치획득
        {            
            Item exp = ItemDatabase.Instance.GetItem(1011);
            exp.count = Random.Range(15, 35);
            ItemGetPanelSlot itemGetPanelSlot_e = PoolManager.Instance.Get_ItemGetPanelSlot();
            itemGetPanelSlot_e.Init_J(exp);
            UIManager.Instance.AddGetSlot_J(itemGetPanelSlot_e);
            itemGetPanelSlot_e.gameObject.SetActive(true);
            itemGetPanelSlot_e.Destroy();
        }
        StartCoroutine(DelayMora());
    }

    IEnumerator DelayMora()
    {
        yield return new WaitForSeconds(1f);
        //모라획득
        {
            Item mora = ItemDatabase.Instance.GetItem(1010);
            mora.count = Random.Range(50, 151);
            InventoryManager.Instance.GetItem(mora);
            ItemGetPanelSlot itemGetPanelSlot = PoolManager.Instance.Get_ItemGetPanelSlot();
            itemGetPanelSlot.Init_J(mora);
            UIManager.Instance.AddGetSlot_J(itemGetPanelSlot);
            itemGetPanelSlot.gameObject.SetActive(true);
            itemGetPanelSlot.Destroy();
        }
        yield break;
    }

    protected virtual IEnumerator Die(Enemy enemy, Character attacker)
    {
        enemy.gameObject.layer = (int)EnemyLayer.isDead;
        enemy.animator.SetTrigger("Die");
        DropElement(enemy);
        DropItem(enemy);

        if(attacker != null)
        {
            attacker.OnEnemyKilled();
        }

        yield return new WaitForSeconds(1.05f);
        enemy.gameObject.SetActive(false);
    }

    protected virtual IEnumerator HitMotion(Enemy enemy)
    {
        Material mat = enemy.EnemyMesh.material;
        enemy.EnemyMesh.material = enemy.EnemyMesh.materials[1];
        yield return new WaitForSeconds(0.1f);
        enemy.EnemyMesh.material = mat;
        isHitMotion = true;
    }

    public float Distance()
    {
        return Vector3.Distance(Player.position, transform.position);
    }

    public void Trace()
    {
        if(Distance() <= traceDistance)
        {
            state.ChangeState(EnemyState.TraceMove);
        }
    }

    public void MoveAnimation(float setFloat)
    {
        animator.SetFloat("Move", setFloat);
    }

    public void SetDestination_Player()
    {
        agent.SetDestination(Player.position);
    }

    public void SetDestination_This()
    {
        agent.SetDestination(transform.position);
    }

    public void TraceAttackRotation()
    {
        Vector3 direction = Player.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }
}


public struct EnemyData
{
    public float Health { get; }
    public float AttackPower { get; }
    public float Speed { get; }
    public float Defence { get; }
    public int DropExp { get; }
    public Element element { get; }

    public EnemyData(float health, float attackPower, float speed, float defence, int dropExp, Element element) //체력 , 공격력, 이동속도, 물리내성, 경험치, 속성
    {
        this.Health = health;
        this.AttackPower = attackPower;
        this.Speed = speed;
        this.Defence = defence;
        this.DropExp = dropExp;
        this.element = element;
    }
    
}
