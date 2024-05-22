using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftAttack : IPattern
{
    private Wolf m_Wolf;
    public DriftAttack(Wolf wolf)
    {
        m_Wolf = wolf;
    }
    public void BossAttack()
    {
        DriftAnimation();
    }

    private float TargetPosition()
    {
        Vector3 targetDirection = (m_Wolf.PlayerTransform.position - m_Wolf.transform.position).normalized;
        Vector3 selfDirection = m_Wolf.transform.forward;

        float angle = Vector3.SignedAngle(selfDirection, targetDirection, Vector3.up);

        return angle;
    }

    private void DriftAnimation()
    {
        float angle = TargetPosition();

        if(angle < 0)
        {
            m_Wolf.BossAnimator.SetTrigger("DriftR");
        }
        else
        {
            m_Wolf.BossAnimator.SetTrigger("DriftL");
        }
    }
}
