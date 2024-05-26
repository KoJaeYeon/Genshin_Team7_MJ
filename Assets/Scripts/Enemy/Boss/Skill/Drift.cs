using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Drift : BossSkill
{
    private float drift_Atk;
    private SphereCollider sphereColl;

    private void OnEnable()
    {
        if(sphereColl == null)
        {
            sphereColl = GetComponent<SphereCollider>();
        }
    }

    public override void SetAtk(float atk)
    {
        drift_Atk = GetSkillDamage(Skill.Drift);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Drift");
            Character player = other.gameObject.GetComponentInChildren<Character>();
            player.TakeDamage(drift_Atk);
        }
    }


}
