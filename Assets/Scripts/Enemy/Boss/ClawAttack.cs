using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawAttack : IPattern
{
    enum RandomAttack
    {
        Claw,
        Claw_Drift
    }

    private Wolf m_Wolf;
    public ClawAttack(Wolf wolf)
    {
        m_Wolf = wolf;
    }

    public void BossAttack()
    {
        RandomAnimation();
    }

    public float TargetPosition()
    {
        Vector3 targetDirection = (m_Wolf.PlayerTransform.position - m_Wolf.transform.position).normalized;
        Vector3 selfDirection = m_Wolf.transform.forward;

        float angle = Vector3.SignedAngle(selfDirection, targetDirection, Vector3.up);

        return angle;
    }

    public void RandomAnimation()
    {
        int randomAnimation = Random.Range(0, 2);
        float playerPosition = TargetPosition();
        
        switch(randomAnimation)
        {
            case (int)RandomAttack.Claw:
                if(playerPosition > 0)
                {
                    m_Wolf.BossAnimator.SetTrigger("ClawL");
                }
                else if(playerPosition < 0)
                {
                    m_Wolf.BossAnimator.SetTrigger("ClawR");
                }
                else
                {
                    m_Wolf.State.ChangeState(BossState.Attack);
                }
                break;
            case (int)RandomAttack.Claw_Drift:
                if (playerPosition > 0)
                {
                    m_Wolf.BossAnimator.SetTrigger("ClawL_Drift");
                }
                else if (playerPosition < 0)
                {
                    m_Wolf.BossAnimator.SetTrigger("ClawR_Drift");
                }
                else
                {
                    m_Wolf.State.ChangeState(BossState.Attack);
                }
                break;
        }

    }
    
}
