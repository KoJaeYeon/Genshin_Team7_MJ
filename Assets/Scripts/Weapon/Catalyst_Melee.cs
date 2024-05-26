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
        SoundManager.Instance.PlayEffect("Wrio_Attack");
    }

    private void PerformMeleeAttack()
    {
        Collider[] hitEnemiesLeftHand = Physics.OverlapSphere(leftHandAttackPoint.position, attackRange, enemyLayer);
        Collider[] hitEnemiesRightHand = Physics.OverlapSphere(rightHandAttackPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemiesLeftHand)
        {
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                Element currentElement = character != null ? character.GetCurrentWeaponElement() : Element.Normal;
                enemyComponent.TakeDamage(attackDamage, currentElement, character);
            }
        }

        foreach (Collider enemy in hitEnemiesRightHand)
        {
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                Element currentElement = character != null ? character.GetCurrentWeaponElement() : Element.Normal;
                enemyComponent.TakeDamage(attackDamage, currentElement, character);
            }
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
