using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySequence : INode
{
    private List<INode> _childNode;

    public EnemySequence(List<INode> childNode)
    {
        _childNode = childNode;
    }

    public INode.NodeState Evaluate()
    {
        if(_childNode == null || _childNode.Count == 0)
        {
            return INode.NodeState.Fail;
        }

        foreach(var child in _childNode)
        {
            switch (child.Evaluate())
            {
                case INode.NodeState.Running:
                    return INode.NodeState.Running;
                case INode.NodeState.Success:
                    continue;
                case INode.NodeState.Fail:
                    return INode.NodeState.Fail;
            }
        }

        return INode.NodeState.Success;
    }
}
