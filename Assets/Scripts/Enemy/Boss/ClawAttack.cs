using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawAttack : IPattern
{
    private Wolf m_Wolf;
    public ClawAttack(Wolf wolf)
    {
        m_Wolf = wolf;
    }

    public void BossAttack()
    {
        m_Wolf.BossAnimator.SetTrigger("");
    }
    
}
