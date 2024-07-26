using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : INode
{
    private Func<INode.NodeState> _onUpdate;

    public EnemyAction(Func<INode.NodeState> onUpdate)
    {
        _onUpdate = onUpdate;
    }

    public INode.NodeState Evaluate()
    {
        if(_onUpdate == null)
        {
            return INode.NodeState.Fail;
        }
        else
        {
            return _onUpdate.Invoke();
        }
    }
}
