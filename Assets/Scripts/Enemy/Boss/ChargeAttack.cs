using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChargeAttack : IPattern
{
    private Wolf m_Wolf;
    private float currentAngle;
    private float distance;
    private Vector3 targetPos;

    
    public ChargeAttack(Wolf wolf)
    {
        m_Wolf = wolf;
        targetPos = m_Wolf.PlayerTransform.position;
        m_Wolf.BossAnimator.SetBool("isRun", true);
        
    }

    public void BossAttack()
    {
        distance = Vector3.Distance(m_Wolf.transform.position, targetPos);

        if (distance > 2.0f)
        {
            Rotation();
        }
        else
        {
            m_Wolf.BossAnimator.SetBool("isRun", false);
            SelectAnimation();
            m_Wolf.BossRigid.velocity = Vector3.zero;
        }
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
        if (currentAngle > 0 && currentAngle <= 180 && !m_Wolf.IsRunStop)
        {
            m_Wolf.IsRunStop = true;
            m_Wolf.BossAnimator.Play("ScramR");
            m_Wolf.State.ChangeState(BossState.Attack);
        }
        else if (currentAngle < 0 && currentAngle >= -180 && !m_Wolf.IsRunStop)
        {
            m_Wolf.IsRunStop = true;
            m_Wolf.BossAnimator.Play("ScramL");
            m_Wolf.State.ChangeState(BossState.Attack);
        }
        else
        {
            m_Wolf.IsRunStop = true;
            m_Wolf.BossAnimator.Play("ScramL");
            m_Wolf.State.ChangeState(BossState.Attack);
        }
    }

}
