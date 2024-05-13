using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    public GameObject itemSlotPrefab;

    Stack<GameObject> itemSlotList;

    public Transform PoolParent;

    private void Awake()
    {
        itemSlotList = new Stack<GameObject>();

        for(int i = 0; i < 200; i++)
        {
            GameObject prefab = Instantiate(itemSlotPrefab,PoolParent.transform);
            prefab.name = i.ToString();
            itemSlotList.Push(prefab);
            prefab.SetActive(false);
        }
    }
    public ItemSlot GetItemSlot()
    {
        ItemSlot itemSlot;
        GameObject prefab;
        if(itemSlotList.TryPop(out prefab))
        {
            itemSlot = prefab.GetComponent<ItemSlot>();
            return itemSlot;
        }
        else
        {
            prefab = Instantiate(itemSlotPrefab, PoolParent.transform);
            itemSlot = prefab.GetComponent<ItemSlot>();
            return itemSlot;
        }
        
        
    }
}
