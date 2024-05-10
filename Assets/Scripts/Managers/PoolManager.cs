using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{

    public ItemSlot GetItemSlot()
    {
        return new ItemSlot();
    }
}
