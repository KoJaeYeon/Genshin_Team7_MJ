using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catalyst : Weapon
{
    public GameObject rangedProjectilePrefab;
    public Transform projectileSpawnPoint;
    public float attackRange = 1.5f;
    public float attackDamage = 20f;
    public LayerMask enemyLayer;

    public override void UseWeapon()
    {
        if(character.characterType == CharacterType.Ranged)
        {
            PerformRangedAttack();
        }
        else if(character.characterType == CharacterType.Melee)
        {
            PerformMeleeAttack();
        }
    }

    private void PerformMeleeAttack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(projectileSpawnPoint.position, attackRange, enemyLayer);
        foreach(Collider enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage, Element.Nomal);
        }
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
