using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Drift : BossSkill
{
    private AndriusDriftData _driftData;
    private float drift_Atk;
    private float driftDamage;
    private SphereCollider sphereColl;

    public Drift()
    {
        _driftData = EnemyCSVLoder.Instance.GetAndriusCSVData<AndriusDriftData>("AndriusDriftData");
        driftDamage = _driftData.SkillDamage;
    }

    private void OnEnable()
    {
        if(sphereColl == null)
        {
            sphereColl = GetComponent<SphereCollider>();
        }
    }

    public override void SetAtk(float atk)
    {
        drift_Atk = driftDamage * atk;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Character player = other.gameObject.GetComponentInChildren<Character>();

            if(player != null)
            {
                player.TakeDamage(drift_Atk);
            }
        }
    }


}
