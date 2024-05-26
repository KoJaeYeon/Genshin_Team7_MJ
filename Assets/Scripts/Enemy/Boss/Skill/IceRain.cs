using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IceRain : BossSkill
{
    private float Ice_Atk;

    public override void SetAtk(float atk)
    {
        Ice_Atk = GetSkillDamage(Skill.Ice_Rain) + atk;
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Character player = other.gameObject.GetComponentInChildren<Character>();
            player.TakeDamage(Ice_Atk);
        }
    }
}
