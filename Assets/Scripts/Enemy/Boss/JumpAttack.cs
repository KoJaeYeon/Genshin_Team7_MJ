using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttack : IPattern
{
    private Wolf m_Wolf;
    public JumpAttack(Wolf wolf)
    {
        m_Wolf = wolf;
    }

    public void BossAttack()
    {
        Debug.Log("Jump ½ÇÇà");
       
    }

}
