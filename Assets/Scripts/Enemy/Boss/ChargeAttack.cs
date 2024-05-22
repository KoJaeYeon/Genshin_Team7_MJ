using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : IPattern
{
    private Wolf m_Wolf;
    private float currentAngle;
    Vector3 targetPos;
    public ChargeAttack(Wolf wolf)
    {
        m_Wolf = wolf;
        targetPos = m_Wolf.PlayerTransform.position;
        m_Wolf.BossAnimator.SetBool("isRun", true);
        
    }

    public void BossAttack()
    {

        if (Vector3.Distance(m_Wolf.transform.position, targetPos) <= 2.0f)
        {
            m_Wolf.BossAnimator.SetBool("isRun", false);
            SelectAnimation();

            m_Wolf.BossRigid.velocity = Vector3.zero;
            m_Wolf.State.ChangeState(BossState.Idle);
        }
        else if (Vector3.Distance(m_Wolf.transform.position, targetPos) > 2.0f)
            Rotation();
            
    }

    private void Rotation()
    {
        Vector3 Pos = targetPos - m_Wolf.transform.position;

        float angle = Mathf.Atan2(Pos.x, Pos.z) * Mathf.Rad2Deg;

        currentAngle = angle;

        Quaternion Rot = Quaternion.Euler(0,angle, 0);

        m_Wolf.transform.rotation = Quaternion.Slerp(m_Wolf.transform.rotation, Rot, 10f * Time.fixedDeltaTime);
    }

    private void SelectAnimation()
    {
        if (currentAngle > 0 && currentAngle <= 180)
            m_Wolf.BossAnimator.SetTrigger("RunStopR");
        else if (currentAngle < 0 && currentAngle >= -180)
            m_Wolf.BossAnimator.SetTrigger("RunStopL");
        else
            m_Wolf.BossAnimator.SetTrigger("RunStopL");
    }

}
