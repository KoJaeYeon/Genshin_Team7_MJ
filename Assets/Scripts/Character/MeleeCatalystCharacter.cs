using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeCatalystCharacter : Character
{
    protected override void Start()
    {
        characterType = CharacterType.Melee;
        base.Start();
        foreach(var weapon in weapons)
        {
            if(weapon is Catalyst_Melee)
            {
                weapon.gameObject.SetActive(true);
                currentWeaponIndex = System.Array.IndexOf(weapons, weapon);
                break;
            }
        }
    }

    private void Update()
    {
        if (_input.attack)
        {
            Attack();
        }
        if (_input.skill && skillCooldownTimer <= 0)
        {
            UseElementalSkill();
            skillCooldownTimer = skillCooldown;
            isSkillActive = true;
            skillDurationTimer = skillDuration;
        }

        if (_input.burst)
        {
            UseElementalBurst();
        }

        if (isSkillActive)
        {
            skillDurationTimer -= Time.deltaTime;

            if (skillDurationTimer <= 0f)
            {
                ResetSkill();
                isSkillActive = false;
            }
        }

        if (skillCooldownTimer > 0f)
        {
            skillCooldownTimer -= Time.deltaTime;
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
        EnchantWeapon(Element.Ice);
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
