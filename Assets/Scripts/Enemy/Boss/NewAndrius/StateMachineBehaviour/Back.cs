using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back : StateMachineBehaviour
{
    private Andrius _andrius;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_andrius == null)
        {
            _andrius = animator.GetComponent<Andrius>();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _andrius.IsAction = false;
    }
}
