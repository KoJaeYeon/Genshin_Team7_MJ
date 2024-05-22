using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catalyst_Melee : Weapon
{
    public LayerMask enemyLayer;
    public Transform leftHandAttackPoint;
    public Transform rightHandAttackPoint;

    public float attackRange = 0.5f;
    public float attackDamage = 20f;

    public override void UseWeapon()
    {
        PerformMeleeAttack();
    }

    private void PerformMeleeAttack()
    {
        Collider[] hitEnemiesLeftHand = Physics.OverlapSphere(leftHandAttackPoint.position, attackRange, enemyLayer);
        Collider[] hitEnemiesRightHand = Physics.OverlapSphere(rightHandAttackPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemiesLeftHand)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage, Element.Nomal);
        }

        foreach (Collider enemy in hitEnemiesRightHand)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage, Element.Nomal);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (leftHandAttackPoint == null || rightHandAttackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(leftHandAttackPoint.position, attackRange);
        Gizmos.DrawWireSphere(rightHandAttackPoint.position, attackRange);
    }
}
