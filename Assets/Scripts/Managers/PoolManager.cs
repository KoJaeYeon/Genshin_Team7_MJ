using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    public GameObject itemSlotPrefab;
    public GameObject itemDropPrefab;

    Stack<GameObject> itemSlotStack;
    Stack<GameObject> itemDropStack;

    public Transform PoolParent;

    private void Awake()
    {
        itemSlotStack = new Stack<GameObject>();

        for(int i = 0; i < 200; i++)
        {
            GameObject prefab = Instantiate(itemSlotPrefab,PoolParent.transform);
            prefab.name = i.ToString();
            itemSlotStack.Push(prefab);
            prefab.SetActive(false);
        }
    }
    public ItemSlot GetItemSlot()
    {
        ItemSlot itemSlot;
        GameObject prefab;
        if(itemSlotStack.TryPop(out prefab))
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
