using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    // Start is called before the first frame update
    void Start()
    {
        GiveInitItem();
    }

    void GiveInitItem()
    {
        InventoryManager.Instance.InitItemSet();

    }
}
