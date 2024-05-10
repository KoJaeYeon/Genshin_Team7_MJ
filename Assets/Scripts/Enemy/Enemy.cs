using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Enemy : MonoBehaviour
{
    protected EnemyStateMachine state;
    protected Dictionary<Enemy, float> EnemyHealthDic;
    protected EnemyData enemyData;

    protected virtual void Awake()
    {
        EnemyHealthDic = new Dictionary<Enemy, float>();
    }

    public void Damaged(Enemy enemy,int damage)
    {
        EnemyHealthDic[enemy] -= damage;

        if (EnemyHealthDic[enemy] <= 0)
        {
            enemy.gameObject.SetActive(false);
        }
    }
}

public struct EnemyData
{
    public float Health { get; }
    public float AttackPower { get; }
    public float Speed { get; }
    public float Defence { get; }
    public int DropExp { get; }
    public EnemyData(float health, float attackPower, float speed, float defence, int dropExp)
    {
        this.Health = health;
        this.AttackPower = attackPower;
        this.Speed = speed;
        this.Defence = defence;
        this.DropExp = dropExp;
    }
    
}
