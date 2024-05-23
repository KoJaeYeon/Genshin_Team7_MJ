using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill : MonoBehaviour
{
    
    void Start()
    {
        InitSkill();
    }

    private void InitSkill()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        



    }

    private IEnumerator DamageDelay()
    {
        yield return new WaitForSeconds(0.5f);
    }
}

public struct SkillDamage
{
    
}
