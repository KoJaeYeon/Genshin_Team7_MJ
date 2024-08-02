using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttack : IPattern
{
    private Wolf m_Wolf;
    private Vector3 EndPos;
    private float Speed = 12.0f;
    private AnimatorStateInfo animatorStateInfo;
    
    public void InitPattern(Wolf wolf)
    {
        if(m_Wolf == null)
        {
            m_Wolf = wolf;
        }

        m_Wolf.BossAnimator.SetTrigger("JumpAttack");
        EndPos = m_Wolf.PlayerTransform.position;
    }

    public void BossAttack()
    {
        animatorStateInfo = m_Wolf.BossAnimator.GetCurrentAnimatorStateInfo(0);

        if (animatorStateInfo.normalizedTime < 0.3f)
        {
            Rotation();
            Move();
        }
    }
    private void Rotation()
    {
        Vector3 targetPos = EndPos - m_Wolf.transform.position;

        float angle = Mathf.Atan2(targetPos.x, targetPos.z) * Mathf.Rad2Deg;

        Quaternion Rot = Quaternion.Euler(0, angle, 0);

        m_Wolf.transform.rotation = Quaternion.Slerp(m_Wolf.transform.rotation, Rot, 5f * Time.fixedDeltaTime);
    }

    private void Move()
    {
        Vector3 Dir = (EndPos - m_Wolf.transform.position).normalized;

        Vector3 move = Dir * Speed * Time.fixedDeltaTime;

        if (Vector3.Distance(m_Wolf.transform.position, EndPos) > move.magnitude)
        {
            m_Wolf.transform.Translate(move, Space.World);
        }
        else
            m_Wolf.transform.position = EndPos;
        
    }

    
}
