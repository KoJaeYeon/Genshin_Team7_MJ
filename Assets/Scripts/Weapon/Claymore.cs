using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claymore : Weapon
{
    public float attackRange = 2.0f;
    public float attackDamge = 30f;
    public Transform attackPoint;
    public LayerMask enemyLayer;

    public override void UseWeapon()
    {
        PerformMeleeAttack();
    }

    public void PerformMeleeAttack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        foreach(Collider enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamge, Element.Nomal);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
