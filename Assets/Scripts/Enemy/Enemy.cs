using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Enemy : MonoBehaviour
{
    protected EnemyStateMachine state;
    protected Dictionary<int, float> enemyHealthDic;
    protected Dictionary<GameObject, float> enemyAtkDic;



    protected virtual void Awake()
    {
        state = gameObject.AddComponent<EnemyStateMachine>();
        enemyHealthDic = new Dictionary<int, float>();
    }

    protected virtual void Damaged(int damage)
    {

    }
}

public class Monster : Enemy
{
    private void Awake()
    {
        enemyHealthDic.Add(gameObject.GetInstanceID(), 100.0f);
    }
}
