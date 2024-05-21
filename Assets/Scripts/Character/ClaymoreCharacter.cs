using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaymoreCharacter : Character
{
    //private MeleeWeapon meleeWeapon;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        //meleeWeapon = GetComponent<MeleeWeapon>();
    }
    public override void Attack()
    {
        weapons[currentWeaponIndex].gameObject.SetActive(true);
        weapons[currentWeaponIndex].UseWeapon();
    }

    protected override void Update()
    {
        base.Update();
        UpdateAttackState();
    }

    private void UpdateAttackState()
    {
        if(animator != null)
        {
            weapons[currentWeaponIndex].Attack = animator.GetBool("Attacking");
        }
    }
}
