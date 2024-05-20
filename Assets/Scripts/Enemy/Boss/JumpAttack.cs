using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttack : IPattern
{
    private Wolf m_Wolf;
    public JumpAttack(Wolf wolf)
    {
        m_Wolf = wolf;
    }

    public void BossAttack()
    {
        m_Wolf.BossAnimator.SetTrigger("JumpAttack");



        if (m_Wolf.BossAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            m_Wolf.State.ChangeState(BossState.Idle);

    }

}
