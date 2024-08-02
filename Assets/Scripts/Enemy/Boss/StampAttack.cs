using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampAttack : IPattern
{
    private Wolf m_Wolf;
    private Vector3 targetPos;
    private float Rotspeed = 15.0f;
    private AnimatorStateInfo animatorStateInfo;

    public void InitPattern(Wolf wolf)
    {
        if(m_Wolf == null)
        {
            m_Wolf = wolf;
        }

        targetPos = m_Wolf.PlayerTransform.position;
        m_Wolf.BossAnimator.SetTrigger("Stamp");
    }

    public void BossAttack()
    {
        animatorStateInfo = m_Wolf.BossAnimator.GetCurrentAnimatorStateInfo(0);

        if(animatorStateInfo.normalizedTime < 0.4f)
        {
            Rotation();
        }
    }


    private void Rotation()
    {
        Vector3 attackPos = targetPos - m_Wolf.transform.position;

        float rotAngle = Mathf.Atan2(attackPos.x, attackPos.z) * Mathf.Rad2Deg;

        Quaternion rot = Quaternion.Euler(0, rotAngle, 0);

        m_Wolf.transform.rotation = Quaternion.Slerp(m_Wolf.transform.rotation, rot, Rotspeed * Time.fixedDeltaTime);
    }

}
