using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : IPattern
{
    private Wolf m_Wolf;
    private bool Change;
    Vector3 targetPos;
    public ChargeAttack(Wolf wolf)
    {
        m_Wolf = wolf;
        //targetPos = new Vector3(m_Wolf.PlayerTransform.position.x, m_Wolf.PlayerTransform.position.y, m_Wolf.PlayerTransform.position.z + 10);
        targetPos = m_Wolf.PlayerTransform.position;
        m_Wolf.BossAnimator.SetBool("isRun", true);
        Change = false;
    }

    public void BossAttack()
    {

        if (Vector3.Distance(m_Wolf.transform.position, targetPos) <= 2.0f && !Change)
        {
            Change = true;
            m_Wolf.BossAnimator.SetBool("isRun", false);
            m_Wolf.BossRigid.velocity = Vector3.zero;
            m_Wolf.State.ChangeState(BossState.Attack);
        }
        else if (Vector3.Distance(m_Wolf.transform.position, targetPos) > 2.0f)
            Rotation();
            
    }

    private void Rotation()
    {
        Vector3 Pos = targetPos - m_Wolf.transform.position;

        float angle = Mathf.Atan2(Pos.x, Pos.z) * Mathf.Rad2Deg;

        Quaternion Rot = Quaternion.Euler(0, angle, 0);

        m_Wolf.transform.rotation = Rot;
    }

}
