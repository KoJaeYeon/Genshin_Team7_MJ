using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface INode
{
    public enum NodeState
    {
        Running,
        Success,
        Fail
    }

    public NodeState Evaluate();
}

public class Node
{
    private INode _root;

    public Node(INode root)
    {
        _root = root;
    }

    public void Execute()
    {
        _root.Evaluate();
    }
}
