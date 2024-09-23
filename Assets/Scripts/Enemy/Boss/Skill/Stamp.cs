using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamp : BossSkill
{
    private float stamp_Atk;
    private float stampDamage;
    private BoxCollider boxColl;
    private AndriusStampData _stampData;

    public Stamp()
    {
        _stampData = EnemyCSVLoader.Instance.GetAndriusCSVData<AndriusStampData>("AndriusStampData");
        stampDamage = _stampData.SkillDamage;
    }

    private void OnEnable()
    {
        if(boxColl == null)
            boxColl = GetComponent<BoxCollider>();
    }

    public override void SetAtk(float atk)
    {
        stamp_Atk = stampDamage * atk;
    }

    public override IEnumerator DelayDamage()
    {
        yield return new WaitForSeconds(1.3f);
        boxColl.enabled = true;
        yield return new WaitForSeconds(1.0f);
        boxColl.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Character player = other.gameObject.GetComponentInChildren<Character>();

            if(player != null)
            {
                player.TakeDamage(stamp_Atk);
            }
        }
    }

}
