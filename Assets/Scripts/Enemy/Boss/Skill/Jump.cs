using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : BossSkill
{
    private float jump_Atk;
    private SphereCollider sphereColl;

    private void OnEnable()
    {
        if(sphereColl == null)
            sphereColl = GetComponent<SphereCollider>();
    }

    public override void SetAtk(float atk)
    {
        jump_Atk = skillDic[Skill.Jump] * atk;
    }
    

    public override IEnumerator DelayDamage()
    {
        yield return new WaitForSeconds(1.0f);
        sphereColl.enabled = true;
        yield return new WaitForSeconds(1.0f);
        sphereColl.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Jump");
            Character player = other.gameObject.GetComponentInChildren<Character>();
            player.TakeDamage(jump_Atk);
        }
    }
}
