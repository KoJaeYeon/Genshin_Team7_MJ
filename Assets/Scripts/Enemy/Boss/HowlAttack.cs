using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowlAttack : IPattern
{
    private Wolf m_Wolf;
    public HowlAttack(Wolf wolf)
    {
        m_Wolf = wolf;
        m_Wolf.BossAnimator.SetTrigger("Howl");
    }
    public void BossAttack()
    {
        
    }

}
