using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public Collider weaponCollider;

    private void Start()
    {
        weaponCollider.enabled = false;
        weaponCollider.isTrigger = true;
    }

    public override void UseWeapon()
    {
        StartCoroutine(PerformAttack());
    }

    private IEnumerator PerformAttack()
    {
        gameObject.SetActive(true);
        weaponCollider.enabled = true;
        Attack = true;
        yield return new WaitForSeconds(attackDuration);
        Attack = false;
        weaponCollider.enabled = false;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(Attack && other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.Damaged(enemy, damage, Element.Nomal);
            }
        }
    }
}
