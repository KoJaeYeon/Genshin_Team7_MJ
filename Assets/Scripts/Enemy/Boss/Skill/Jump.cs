using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : BossSkill
{
    private float jump_Atk;
    private SphereCollider sphereColl;

    public override void AnimationEventStart()
    {
        Debug.Log("점프 애니메이션 스타트");
        jump_Atk = GetSkillDamage(Skill.Jump);
        sphereColl = GetComponent<SphereCollider>();

        StartCoroutine(DelayDamage());
    }

    public override void AnimationEventEnd()
    {
        sphereColl.enabled = false;
    }

    public override IEnumerator DelayDamage()
    {
        yield return new WaitForSeconds(1.0f);
        sphereColl.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Jump");
        }
    }
}
