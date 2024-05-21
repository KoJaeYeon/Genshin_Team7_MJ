using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : Health
{
    protected override void Start()
    {
        Debug.Log("플레이어 데이터에서 참조할 예정");
    }

    protected override void Die()
    {
        base.Die();
    }
}
