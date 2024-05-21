using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catalyst : Weapon
{
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
        throw new NotImplementedException();
    }

    private void PerformRangedAttack()
    {
        throw new NotImplementedException();
    }
}
