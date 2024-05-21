using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaymoreCharacter : Character
{
    private bool isAttacking = false;
    private Animator animator;
    private int damage = 10;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public override void Attack()
    {
        MeleeAttack();
    }

    private void MeleeAttack()
    {
        Debug.Log("MeleeAttack With Claymore");
        isAttacking = true;
        Invoke("StopMeleeAttack", 1.0f);
    }

    private void StopMeleeAttack()
    {
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isAttacking && other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.Damaged(enemy, damage, Element.Nomal);
                Debug.Log("Damaged");
            }
        }
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
            isAttacking = animator.GetBool("Attacking");
        }
    }
}
