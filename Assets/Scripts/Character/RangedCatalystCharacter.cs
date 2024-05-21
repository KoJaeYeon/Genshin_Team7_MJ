using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedCatalystCharacter : CatalystCharacter
{
    protected override void Start()
    {
        characterType = CharacterType.Ranged;
        base.Start();
        foreach(var weapon in weapons)
        {
            if(weapon is Catalyst)
            {
                weapon.gameObject.SetActive(true);
                currentWeaponIndex = System.Array.IndexOf(weapons, weapon);
                break;
            }
        }
    }
}
