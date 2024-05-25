using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class BowCharacter : Character
{
    protected override void Start()
    {
        characterType = CharacterType.Ranged;
        base.Start();
        foreach(var weapon in weapons)
        {
            if(weapon is Bow)
            {
                weapon.gameObject.SetActive(true);
                currentWeaponIndex = System.Array.IndexOf(weapons, weapon);
                break;
            }
        }
    }

    public override void Attack()
    {
        if (weapons.Length > 0)
        {
            weapons[currentWeaponIndex].UseWeapon();
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
}
