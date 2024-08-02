using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPattern
{
    public void InitPattern(Wolf wolf);
    public void BossAttack();
}
