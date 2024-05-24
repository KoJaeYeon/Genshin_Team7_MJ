using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Skill
{
    Jump,
    Howl,
    Stamp,
    Drift,
    Claw
}
public class BossSkill : MonoBehaviour
{
    protected Dictionary<Skill, float> skillDic;
    protected Wolf wolf;
    protected bool Hit;

    private void Start()
    {
        Debug.Log("보스 스킬 호출");
        skillDic = new Dictionary<Skill, float>();
        wolf = GetComponent<Wolf>();

        skillDic.Add(Skill.Howl, 20.0f);
        skillDic.Add(Skill.Claw, 20.0f);
        skillDic.Add(Skill.Jump, 20.0f);
        skillDic.Add(Skill.Drift, 20.0f);
        skillDic.Add(Skill.Stamp, 20.0f);
    }

    public float GetSkillDamage(Skill name)
    {
        float damage = skillDic[name] + wolf.GetAtk();

        return damage;
    }

    public virtual void AnimationEventStart() { }
    public virtual void AnimationEventEnd() { }
    public virtual IEnumerator DelayDamage()
    {
        yield return new WaitForSeconds(1.0f);
    }
}


