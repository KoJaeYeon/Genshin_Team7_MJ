using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();//�÷��̾� Transform ���� ����� ���⸸ �����ϸ� �˴ϴ�.
        Weapon = transform.GetComponentInChildren<MonsterWeapon>(); //���� ����
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        HpSlider = transform.GetComponentInChildren<Slider>();
        Hp = HpSlider.gameObject;

        EnemyHealthDic = new Dictionary<Enemy, float>();
    }

    public abstract void Damaged(Enemy enemy, float damage, Element element);
    public abstract void Splash(float damage);

    protected float Armor(Enemy enemy,float damage, Element element) //���Ұ� �߰��Ǹ� ���ҿ� ���� �ٸ� ���� ���� ����..
    {
        switch (element)
        {
            case Element.Fire:
                if(enemy.enemyData.element == Element.Ice)
                {
                    Debug.Log("����");
                    damage *= 2f;
                }
                else if(enemy.enemyData.element == Element.Lightning)
                {
                    Debug.Log("����ȭ");
                    damage -= damage * enemy.enemyData.Defence;
                    SplashAttack(enemy);
                }
                else
                {
                    damage -= damage * enemy.enemyData.Defence;
                }
                break;
            case Element.Ice:
                if(enemy.enemyData.element == Element.Fire)
                {
                    Debug.Log("����");
                    damage *= 1.5f;
                }
                else if(enemy.enemyData.element == Element.Lightning)
                {
                    Debug.Log("������");
                    damage -= damage * enemy.enemyData.Defence;
                    SplashAttack(enemy);
                }
                else
                {
                    damage -= damage * enemy.enemyData.Defence;
                }
                break;
            case Element.Lightning:
                if(enemy.enemyData.element == Element.Fire)
                {
                    Debug.Log("����ȭ");
                    damage -= damage * enemy.enemyData.Defence;
                    SplashAttack(enemy);
                }
                else if(enemy.enemyData.element == Element.Ice)
                {
                    Debug.Log("������");
                    damage -= damage * enemy.enemyData.Defence;
                    SplashAttack(enemy);
                }
                else
                {
                    damage -= damage * enemy.enemyData.Defence;
                }
                break;
            case Element.Nomal:
                damage -= damage * enemy.enemyData.Defence;
                break;
        }
        return damage;
    }

    private void SplashAttack(Enemy enemy)
    {
        Collider[] collider = Physics.OverlapSphere(enemy.transform.position, 2.0f, LayerMask.GetMask("Monster"));

        for(int i = 0; i < collider.Length; i++)
        {
            Enemy checkEnemy = collider[i].GetComponent<Enemy>();

            if (checkEnemy != null)
            {
                checkEnemy.Splash(5f);
            }
        }
    }

    private int ReturnExp(Enemy enemy) //����ġ
    {
        int Exp = enemy.enemyData.DropExp;

        return Exp;
    }

    private void DropElement(Enemy enemy)
    {
        for(int i = 0; i < elementCount; i++) //���߿� Ǯ�Ŵ������� ������� �ڵ�� �����ؾ���!!
        {
            GameObject dropElement = ElementPool.Instance.GetElementObject();
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
            case Element.Nomal:
                ElementColor = Color.white;
                break;
        }

        for(int i = 0; i < elementCount; i++)
        {
            GameObject hitElement = ElementPool.Instance.GetElementObject();
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

    }

    protected IEnumerator Die(Enemy enemy)
    {
        enemy.gameObject.layer = (int)EnemyLayer.isDead;
        enemy.animator.SetTrigger("Die");
        DropElement(enemy);
        DropItem(enemy);

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

    public EnemyData(float health, float attackPower, float speed, float defence, int dropExp, Element element) //ü�� , ���ݷ�, �̵��ӵ�, ��������, ����ġ, �Ӽ�
    {
        this.Health = health;
        this.AttackPower = attackPower;
        this.Speed = speed;
        this.Defence = defence;
        this.DropExp = dropExp;
        this.element = element;
    }
    
}
