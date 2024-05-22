using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowlAttack : IPattern
{
    private Wolf m_Wolf;
    private AnimatorStateInfo stateInfo;
    public HowlAttack(Wolf wolf)
    {
        m_Wolf = wolf;
        m_Wolf.BossAnimator.SetBool("isHowl", true);
    }
    public void BossAttack()
    {
        stateInfo = m_Wolf.BossAnimator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("HowlAS") && stateInfo.normalizedTime >= 1.0f)
        {
            Debug.Log("Howl Ż�⹮");
            m_Wolf.BossAnimator.SetBool("isHowl", false);
            m_Wolf.State.ChangeState(BossState.Attack);
        }
            
    }

}
