using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claymore : Weapon
{
    public override void UseWeapon()
    {
        PerformMeleeAttack();
    }

    public void PerformMeleeAttack()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            
        }
    }
}
