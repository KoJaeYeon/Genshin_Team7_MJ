using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Animator _animator;
    int _animIDAttackCount = Animator.StringToHash("AttackCount");

    private void Start()
    {
        TryGetComponent(out _animator);
    }

    public int AttackCount
    {
        get => _animator.GetInteger(_animIDAttackCount);
        set => _animator.SetInteger(_animIDAttackCount, value);
    }
}
