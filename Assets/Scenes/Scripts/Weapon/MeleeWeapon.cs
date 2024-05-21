using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public override void UseWeapon()
    {
        StartCoroutine(PerformAttack());
    }

    private IEnumerator PerformAttack()
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        gameObject.SetActive(false);
    }
}
