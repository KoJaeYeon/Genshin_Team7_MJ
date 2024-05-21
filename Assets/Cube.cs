using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : Enemy
{
    public override void Damaged(Enemy enemy, float damage, Element element)
    {
        EnemyHealthDic[this] -= Armor(enemy, damage, element);
        HpSlider.value = EnemyHealthDic[this];
        transform.LookAt(Player.position);
        animator.SetTrigger("Hit");

        if (EnemyHealthDic[this] <= 0)
        {
            Hp.SetActive(false);
            StartCoroutine(Die(this));
        }
        else
            HitDropElement(element);
    }

    public override void Splash(float damage)
    {
        EnemyHealthDic[this] -= damage;
        HpSlider.value = EnemyHealthDic[this];
        if (EnemyHealthDic[this] <= 0)
        {
            StartCoroutine(Die(this));
        }
    }
}
