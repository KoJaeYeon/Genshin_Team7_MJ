using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
public abstract class Enemy : MonoBehaviour
{
    protected EnemyStateMachine state;
    protected BossStateMachine bossState;
    protected MonsterWeapon Weapon;
    protected Animator animator;
    protected Transform Player;
    protected NavMeshAgent agent;
    protected Dictionary<Enemy, float> EnemyHealthDic;
    protected GameObject Hp;
    protected Slider HpSlider;

    private IColor color;
    private Color ElementColor;
    protected Element HitElement;
    protected EnemyData enemyData;
    protected float traceDistance = 5.0f;
    protected bool traceMove = true;
    protected bool attack = true;
    private int elementCount = 5;

    protected virtual void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Weapon = transform.GetComponentInChildren<MonsterWeapon>(); 
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        HpSlider = transform.GetComponentInChildren<Slider>();
        Hp = HpSlider.gameObject;

        EnemyHealthDic = new Dictionary<Enemy, float>();
    }

    public virtual void TakeDamage(float damage, Element element, Character attacker)
    {
        EnemyHealthDic[this] -= CalculateDamage(damage, element);
        HpSlider.value = EnemyHealthDic[this];
        transform.LookAt(Player.position);
        animator.SetTrigger("Hit");
        PoolManager.Instance.Get_Text(damage, transform.position);

        if (EnemyHealthDic[this] <= 0)
        {
            Hp.SetActive(false);
            StartCoroutine(Die(this, attacker));
        }
        else
            if (element != Element.Normal) HitDropElement(element);
    }

    public abstract void Splash(float damage);

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

    private int ReturnExp(Enemy enemy) //경험치
    {
        int Exp = enemy.enemyData.DropExp;

        return Exp;
    }

    private void DropElement(Enemy enemy)
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
        }
    }

    public Color EnemyGetColor(Enemy enemy)
    {
        color = enemy.GetComponent<IColor>();
        ElementColor = color.GetColor();
        return ElementColor;
    }

    private void DropItem(Enemy enemy)
    {
        DropObject dropObject = PoolManager.Instance.Get_DropObject(Random.Range(1001, 1007));
        dropObject.gameObject.transform.position = transform.position + Vector3.up*1.5f;
    }

    protected IEnumerator Die(Enemy enemy, Character attacker)
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
