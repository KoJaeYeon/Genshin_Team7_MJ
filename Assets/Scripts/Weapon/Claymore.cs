using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claymore : Weapon
{
    public float attackRange = 0.3f;
    public float attackReach = 1.5f;
    public float attackDamage;
    public Transform attackPoint;
    public LayerMask enemyLayer;

    public override void UseWeapon()
    {
        PerformMeleeAttack();
        SoundManager.Instance.PlayEffect("Beidou_Attack");
    }

    public void PerformMeleeAttack()
    {
        attackDamage = (int)(((PartyManager.Instance.GetCurrentCharacterData().baseAtk + EquipManager.Instance.beidou_Equip.featherDamage + EquipManager.Instance.beidou_Equip.weaponDamage) * (1 + EquipManager.Instance.beidou_Equip.trohphy_AttackPercent / 100f)) / 10f);

        Vector3 startPoint = attackPoint.position;
        Vector3 endPoint = attackPoint.position + attackPoint.forward * attackReach;
        Collider[] hitEnemies = Physics.OverlapCapsule(startPoint, endPoint, attackReach, enemyLayer);

        foreach(Collider enemy in hitEnemies)
        {
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if(enemyComponent != null)
            {
                Element currentElement = character != null ? character.GetCurrentWeaponElement() : Element.Normal;
                enemyComponent.TakeDamage(attackDamage, currentElement, character);

                if(character != null && currentElement != Element.Normal)
                {
                    character.GainEnergy(character.energyGainPerHit);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Vector3 startPoint = attackPoint.position;
        Vector3 endPoint = attackPoint.position + attackPoint.forward * attackReach;

        // Draw the capsule
        Gizmos.DrawWireSphere(startPoint, attackRange);
        Gizmos.DrawWireSphere(endPoint, attackRange);
        Gizmos.DrawLine(startPoint + Vector3.up * attackRange, endPoint + Vector3.up * attackRange);
        Gizmos.DrawLine(startPoint - Vector3.up * attackRange, endPoint - Vector3.up * attackRange);
        Gizmos.DrawLine(startPoint + Vector3.right * attackRange, endPoint + Vector3.right * attackRange);
        Gizmos.DrawLine(startPoint - Vector3.right * attackRange, endPoint - Vector3.right * attackRange);
    }
}
