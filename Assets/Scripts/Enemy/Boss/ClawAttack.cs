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
    private SphereCollider left_Coll;
    private SphereCollider right_Coll;

    public ClawAttack(Wolf wolf) 
    {
        m_Wolf = wolf;
        left_Coll = m_Wolf.left_Hand.GetComponent<SphereCollider>();    
        right_Coll = m_Wolf.right_Hand.GetComponent<SphereCollider>();
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
                    m_Wolf.StartCoroutine(LeftColl());
                }
                else if(playerPosition < 0)
                {
                    m_Wolf.BossAnimator.SetTrigger("ClawR");
                    m_Wolf.StartCoroutine(RightColl());
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
                    m_Wolf.StartCoroutine(LeftColl());
                }
                else if (playerPosition < 0)
                {
                    m_Wolf.BossAnimator.SetTrigger("ClawR_Drift");
                    m_Wolf.StartCoroutine(RightColl());
                }
                else
                {
                    m_Wolf.State.ChangeState(BossState.Attack);
                }
                break;
        }

    }

    private IEnumerator RightColl()
    {
        right_Coll.enabled = true;
        yield return new WaitForSeconds(1.5f);
        right_Coll.enabled = false;
    }

    private IEnumerator LeftColl()
    {
        left_Coll.enabled = true;
        yield return new WaitForSeconds(1.5f);
        left_Coll.enabled = false;
    }
    
}
