using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Drift : BossSkill
{
    public GameObject[] Tail;
    private SphereCollider[] TailSphere;
    private float drift_Atk;

    private void Awake()
    {
        TailSphere = new SphereCollider[Tail.Length];

        for(int i = 0; i < Tail.Length; i++)
        {
            SphereCollider sphereColl = Tail[i].gameObject.GetComponent<SphereCollider>();    

            if(sphereColl != null)
            {
                sphereColl.enabled = false;
                TailSphere[i] = sphereColl;
                TailSphere[i].AddComponent<TailHandler>().Init(this, i);
            }
            
        }
    }

    public void OnTail()
    {
        for(int i = 0; i < Tail.Length; i++)
        {
            TailSphere[i].enabled = true;
            TailSphere[i].isTrigger= true;
        }
    }

    public void OffTail()
    {
        for (int i = 0; i < Tail.Length; i++)
        {
            TailSphere[i].enabled = false;
        }
    }

    public override void SetAtk(float atk)
    {
        drift_Atk = GetSkillDamage(Skill.Drift) + atk;
    }

    public void OnTailTriggerEnter(int index, Collider other)
    {

    }
}
