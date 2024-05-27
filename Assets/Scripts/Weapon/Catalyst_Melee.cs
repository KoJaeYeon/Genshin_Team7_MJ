using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catalyst_Melee : Weapon
{
    public LayerMask enemyLayer;
    public Transform leftHandAttackPoint;
    public Transform rightHandAttackPoint;

    public float attackRange = 0.5f;
    public float attackDamage;

    public override void UseWeapon()
    {
        PerformMeleeAttack();
        SoundManager.Instance.PlayEffect("Wrio_Attack");
    }

    private void PerformMeleeAttack()
    {
        attackDamage = (int)(((PartyManager.Instance.GetCurrentCharacterData().baseAtk + EquipManager.Instance.wrio_Equip.featherDamage + EquipManager.Instance.wrio_Equip.weaponDamage) * (1 + EquipManager.Instance.wrio_Equip.trohphy_AttackPercent / 100f)) / 10f);

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
