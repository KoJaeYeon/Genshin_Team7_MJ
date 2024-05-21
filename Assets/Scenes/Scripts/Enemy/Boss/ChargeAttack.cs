using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : IPattern
{
    private Wolf m_Wolf;
    public ChargeAttack(Wolf wolf)
    {
        m_Wolf = wolf;
    }

    public void BossAttack()
    {
        Debug.Log("Charge ����");
        m_Wolf.State.ChangeState(BossState.Attack); 
    }
}
