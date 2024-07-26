using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackReset : StateMachineBehaviour
{

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}
