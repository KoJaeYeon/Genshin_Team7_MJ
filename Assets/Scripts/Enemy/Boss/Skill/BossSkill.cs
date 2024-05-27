using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Skill
{
    Jump,
    Howl,
    Stamp,
    Drift,
    Ice_Rain
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

    private void Init() //대미지는 *
    {
        skillDic.Add(Skill.Howl, 1.0f);
        skillDic.Add(Skill.Jump, 4.0f);
        skillDic.Add(Skill.Drift, 1.0f);
        skillDic.Add(Skill.Stamp, 2.0f);
        skillDic.Add(Skill.Ice_Rain, 2.0f);
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


