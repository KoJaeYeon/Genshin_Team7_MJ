using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : Health
{
    protected override void Start()
    {
        Debug.Log("�÷��̾� �����Ϳ��� ������ ����");
    }

    protected override void Die()
    {
        base.Die();
    }
}
