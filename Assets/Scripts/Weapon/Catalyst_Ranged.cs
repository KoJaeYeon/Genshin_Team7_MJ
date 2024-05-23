using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catalyst_Ranged : Weapon
{
    public GameObject rangedProjectilePrefab;
    public Transform projectileSpawnPoint;
    public LayerMask enemyLayer;

    public float attackRange = 1.5f;
    public float attackDamage = 20f;

    public override void UseWeapon()
    {
        PerformRangedAttack();
    }

    private void PerformRangedAttack()
    {
        Instantiate(rangedProjectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
    }

    private void OnDrawGizmosSelected()
    {
        if (projectileSpawnPoint == null) return;

        Gizmos.DrawWireSphere(projectileSpawnPoint.position, attackRange);
    }
}
