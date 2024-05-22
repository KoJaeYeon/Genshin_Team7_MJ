//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Catalyst_Melee : Weapon
//{
//    public LayerMask enemyLayer;
//    public Transform leftHandAttackPoint;
//    public Transform rightHandAttackPoint;

//    public float attackRange = 0.5f;
//    public float attackDamage = 20f;

//    public override void UseWeapon()
//    {
//        Debug.Log("@@@");
//        PerformMeleeAttack();
//    }

//    private void PerformMeleeAttack()
//    {
//        Collider[] hitEnemiesLeftHand = Physics.OverlapSphere(leftHandAttackPoint.position, attackRange, enemyLayer);
//        Debug.Log($"{hitEnemiesLeftHand.GetValue(0)}");
//        Collider[] hitEnemiesRightHand = Physics.OverlapSphere(rightHandAttackPoint.position, attackRange, enemyLayer);
//        Debug.Log($"{hitEnemiesRightHand.GetValue(0)}");

//        foreach (Collider enemy in hitEnemiesLeftHand)
//        {
//            Debug.Log("!!!");
//            enemy.GetComponent<Enemy>().TakeDamage(attackDamage, Element.Nomal);
//            Debug.Log("???");
//        }

//        foreach (Collider enemy in hitEnemiesRightHand)
//        {
//            Debug.Log("!!!");
//            enemy.GetComponent<Enemy>().TakeDamage(attackDamage, Element.Nomal);
//            Debug.Log("???");
//        }
//    }

//    private void OnDrawGizmosSelected()
//    {
//        if (leftHandAttackPoint == null || rightHandAttackPoint == null)
//            return;

//        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere(leftHandAttackPoint.position, attackRange);
//        Gizmos.DrawWireSphere(rightHandAttackPoint.position, attackRange);
//    }
//}

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
        Debug.Log("Weapon used!");
        PerformMeleeAttack();
    }

    private void PerformMeleeAttack()
    {
        Collider[] hitEnemiesLeftHand = Physics.OverlapSphere(leftHandAttackPoint.position, attackRange, enemyLayer);
        Collider[] hitEnemiesRightHand = Physics.OverlapSphere(rightHandAttackPoint.position, attackRange, enemyLayer);

        Debug.Log($"Left hand hit count: {hitEnemiesLeftHand.Length}");
        Debug.Log($"Right hand hit count: {hitEnemiesRightHand.Length}");

        foreach (Collider enemy in hitEnemiesLeftHand)
        {
            if (enemy != null)
            {
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    Debug.Log("Left hand attack hit enemy!");
                    enemyComponent.TakeDamage(attackDamage, Element.Nomal);
                }
                else
                {
                    Debug.Log("Left hand hit object without Enemy component.");
                }
            }
        }

        foreach (Collider enemy in hitEnemiesRightHand)
        {
            if (enemy != null)
            {
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    Debug.Log("Right hand attack hit enemy!");
                    enemyComponent.TakeDamage(attackDamage, Element.Nomal);
                }
                else
                {
                    Debug.Log("Right hand hit object without Enemy component.");
                }
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


