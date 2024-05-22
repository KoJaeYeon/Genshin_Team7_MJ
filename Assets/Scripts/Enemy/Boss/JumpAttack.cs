using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttack : IPattern
{
    private Wolf m_Wolf;
    private Vector3 EndPos;
    private bool isJump;
    private AnimatorStateInfo animatorStateInfo;
    public JumpAttack(Wolf wolf)
    {
        m_Wolf = wolf;
        //AnimationStart();
        
    }

    public void BossAttack()
    {
        //if (isJump)
        //{
        //    Rotation();
        //}
        m_Wolf.BossAnimator.SetTrigger("JumpAttack");

        animatorStateInfo = m_Wolf.BossAnimator.GetCurrentAnimatorStateInfo(0);

        if (animatorStateInfo.normalizedTime < 0.4f)
            Rotation();
    }
    private void Rotation()
    {
        Vector3 targetPos = m_Wolf.PlayerTransform.position - m_Wolf.transform.position;

        float angle = Mathf.Atan2(targetPos.x, targetPos.z) * Mathf.Rad2Deg;

        Quaternion Rot = Quaternion.Euler(0, angle, 0);

        m_Wolf.transform.rotation = Quaternion.Slerp(m_Wolf.transform.rotation, Rot, 5.0f * Time.fixedDeltaTime);
    }

    private void AnimationStart()
    {
        m_Wolf.BossAnimator.SetTrigger("JumpAttack");
        isJump = true;
        m_Wolf.StartCoroutine(AnimationStop());
    }

    private IEnumerator AnimationStop()
    {
        yield return new WaitForSeconds(m_Wolf.BossAnimator.GetCurrentAnimatorStateInfo(0).length);
        isJump = false;
        m_Wolf.State.ChangeState(BossState.Attack);
    }

}
