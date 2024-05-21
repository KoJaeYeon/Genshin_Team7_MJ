using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCatalystCharacter : CatalystCharacter
{
    private void OnTriggerEnter(Collider other)
    {
        if(weapons.Length > 0 && weapons[currentWeaponIndex].gameObject.activeSelf)
        {
            
        }
    }

    protected override void Update()
    {
        base.Update();
    }
}
