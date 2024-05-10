using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    public GameObject itemSlotPrefab;

    List<GameObject> itemSlotList;

    public Transform PoolParent;

    private void Awake()
    {
        itemSlotList = new List<GameObject>();

        for(int i = 0; i < 200; i++)
        {
            GameObject prefab = Instantiate(itemSlotPrefab,PoolParent.transform);
            itemSlotList.Add(prefab);
            prefab.SetActive(false);
        }
    }
    public ItemSlot GetItemSlot()
    {
        return new ItemSlot();
    }
}
