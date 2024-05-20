using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawAttack : IPattern
{
    private Wolf m_Wolf;

    AnimatorStateInfo stateInfo;
    public ClawAttack(Wolf wolf)
    {
        m_Wolf = wolf;
    }

    public void BossAttack()
    {
        TargetPosition();

        stateInfo = m_Wolf.BossAnimator.GetCurrentAnimatorStateInfo(0);


        if (stateInfo.IsName("Ani_Monster_LupiBoreas_DriftR") && stateInfo.normalizedTime >= 1.0f)
        {
            Debug.Log("R Ż�⹮");
            m_Wolf.State.ChangeState(BossState.Attack);
        }
        else if (stateInfo.IsName("Ani_Monster_LupiBoreas_DriftL") && stateInfo.normalizedTime >= 1.0f)
        {
            m_Wolf.State.ChangeState(BossState.Attack);
            Debug.Log("L Ż�⹮");
        }
    }

    public void TargetPosition()
    {
        Vector3 targetDirection = (m_Wolf.PlayerTransform.position - m_Wolf.transform.position).normalized;
        Vector3 selfDirection = m_Wolf.transform.forward;

        float angle = Vector3.SignedAngle(selfDirection, targetDirection, Vector3.up);

        if (angle < 0)
            m_Wolf.BossAnimator.SetTrigger("ClawR");
        else if (angle > 0)
            m_Wolf.BossAnimator.SetTrigger("ClawL");
        else
            m_Wolf.BossAnimator.SetTrigger("ClawL");
    }
    
}
