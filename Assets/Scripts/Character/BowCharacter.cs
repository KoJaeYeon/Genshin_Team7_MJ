using UnityEngine;

public class BowCharacter : Character
{
    protected override void Start()
    {
        characterType = CharacterType.Ranged;
        base.Start();
        InitializeBow();
    }

    private void InitializeBow()
    {
        foreach (var weapon in weapons)
        {
            if (weapon is Bow)
            {
                weapon.gameObject.SetActive(true);
                currentWeaponIndex = System.Array.IndexOf(weapons, weapon);
                break;
            }
        }
    }

    public override void Attack()
    {
        PerformAttackAnimation();
        AttackNearestEnemyInRange();
    }

    public override void UseElementalSkill()
    {
        EnchantWeapon(Element.Fire);
    }

    public override void UseElementalBurst()
    {
        if (currentElementalEnergy >= elementalBurstCost)
        {
            currentElementalEnergy -= elementalBurstCost;
        }
    }

    private void EnchantWeapon(Element element)
    {
        if (weapons.Length > 0)
        {
            weapons[currentWeaponIndex].element = element;
        }
    }

    protected override void ResetSkill()
    {
        base.ResetSkill();
    }

    protected override void AttackTarget(Transform target)
    {
        if (weapons.Length > 0)
        {
            Bow bow = weapons[currentWeaponIndex] as Bow;
            if (bow != null)
            {
                bow.UseWeapon(target);
            }

            if (hasAnimator)
            {
                _animator.SetTrigger("Attack");
                _animator.SetBool("Attacking", true);
            }
            else
            {
                _animator.SetBool("Attacking", false);
            }
        }
    }
}
