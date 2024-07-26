using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelector : INode
{
    private List<INode> _childNode;

    public EnemySelector(List<INode> childNode)
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
                    return INode.NodeState.Success;
                case INode.NodeState.Fail:
                    continue;
            }
        }

        return INode.NodeState.Fail;

    }
}
