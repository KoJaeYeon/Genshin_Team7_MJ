using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamp : BossSkill
{
    private float stamp_Atk;
    private BoxCollider boxColl;

    public override void AnimationEventStart()
    {
        stamp_Atk = GetSkillDamage(Skill.Stamp);
        boxColl = GetComponent<BoxCollider>();

        StartCoroutine(DelayDamage());
    }
    public override void AnimationEventEnd()
    {
        boxColl.enabled = false;
    }

    public override IEnumerator DelayDamage()
    {
        yield return new WaitForSeconds(1.0f);
        boxColl.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Stamp");
        }
    }

}
