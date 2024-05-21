using Cinemachine;
using System.Collections;
using System.Collections.Generic;
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
}
