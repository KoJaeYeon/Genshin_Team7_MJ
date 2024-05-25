using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowlAttack : IPattern
{
    private enum SelectHowl
    {
        howl,
        iceRain
    }
    private Wolf m_Wolf;
    public HowlAttack(Wolf wolf)
    {
        m_Wolf = wolf;
        RandomHowl();
    }
    public void BossAttack()
    {
        
    }

    public void RandomHowl()
    {
        int random = Random.Range(0, 2);

        switch (random)
        {
            case (int)SelectHowl.howl:
                m_Wolf.BossAnimator.SetTrigger("Howl");
                break;
            case (int)SelectHowl.iceRain:
                m_Wolf.BossAnimator.SetTrigger("IceRain");
                break;
        }
    }
}
