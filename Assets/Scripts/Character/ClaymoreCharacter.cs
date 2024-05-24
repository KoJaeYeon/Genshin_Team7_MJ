using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClaymoreCharacter : Character
{
    protected override void Start()
    {
        characterType = CharacterType.Melee;
        base.Start();
        foreach(var weapon in weapons)
        {
            if(weapon is Claymore)
            {
                weapon.gameObject.SetActive(true);
                currentWeaponIndex = System.Array.IndexOf(weapons, weapon);
                break;
            }
        }
    }

    public override void Attack()
    {
        if(weapons.Length > 0)
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
        Debug.Log("Lightning");
        EnchantWeapon(Element.Lightning);
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
