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
    protected bool Hit;

    private void Awake()
    {
        skillDic = new Dictionary<Skill, float>();
        Init();
    }

    private void Init()
    {
        skillDic.Add(Skill.Howl, 20.0f);
        skillDic.Add(Skill.Claw, 20.0f);
        skillDic.Add(Skill.Jump, 20.0f);
        skillDic.Add(Skill.Drift, 20.0f);
        skillDic.Add(Skill.Stamp, 20.0f);
    }

    public float GetSkillDamage(Skill name)
    {
        float damage = skillDic[name];

        return damage;
    }

    public virtual void SetAtk(float atk) { }

    public virtual IEnumerator DelayDamage()
    {
        yield return new WaitForSeconds(1.0f);
    }
}


