using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public enum EnemyLayer
{
    isAlive = 3,
    isDead = 6
}

public enum Element
{
    Nomal,
    Fire,
    Ice,
    Lightning
}
public class Enemy : MonoBehaviour
{
    protected EnemyStateMachine state;
    protected MonsterWeapon Weapon;
    protected Animator animator;
    protected Transform Player;
    protected NavMeshAgent agent;
    protected Dictionary<Enemy, float> EnemyHealthDic;

    protected EnemyData enemyData;
    protected float traceDistance = 5.0f;
    protected bool traceMove = true;
    protected bool attack = true;
    private int elementCount = 5;

    protected virtual void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();//플레이어 Transform 참조 변경시 여기만 변경하면 됩니다.
        Weapon = transform.GetComponentInChildren<MonsterWeapon>(); //삭제 예정
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        EnemyHealthDic = new Dictionary<Enemy, float>();
        StartCoroutine(TestDie(this));
    }


    public void Damaged(Enemy enemy, float damage)
    {
        EnemyHealthDic[enemy] -= (damage - Armor(enemy, damage));
        enemy.transform.LookAt(Player.transform.position);
        enemy.animator.SetTrigger("Hit");

        if (EnemyHealthDic[enemy] <= 0)
        {
            //ReturnExp(enemy); 플레이어
            StartCoroutine(Die(enemy));
        }
    }

    private float Armor(Enemy enemy,float damage) //원소가 추가되면 원소에 따라 다른 방어력 구현 예정..
    {
        float armor;

        armor = damage * enemy.enemyData.Defence;
        return armor;
    }

    private int ReturnExp(Enemy enemy) //경험치
    {
        int Exp = enemy.enemyData.DropExp;

        return Exp;
    }

    private void DropElement(Enemy enemy)
    {
        for(int i = 0; i < elementCount; i++)
        {
            GameObject dropElement = ElementPool.Instance.GetElementObject();
            ElementObject elementObject = dropElement.GetComponent<ElementObject>();
            dropElement.transform.position = enemy.transform.position;
            dropElement.SetActive(true);
            StartCoroutine(elementObject.UP());
        }
    }

    private IEnumerator Die(Enemy enemy)
    {
        enemy.gameObject.layer = (int)EnemyLayer.isDead;
        enemy.animator.SetTrigger("Die");
        DropElement(enemy);
        yield return new WaitForSeconds(1.05f);
        enemy.gameObject.SetActive(false);
    }

    private IEnumerator TestDie(Enemy enemy)
    {
        yield return new WaitForSeconds(6.0f);
        Damaged(enemy, 9999);
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
