using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailHandler : MonoBehaviour
{
    private Drift Tail;
    private int Index;

    public void Init(Drift drift, int index)
    {
        Tail = drift;
        Index = index;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Tail.OnTailTriggerEnter(Index, other);
        }
    }
}
