using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    public GameObject itemSlotPrefab;
    public GameObject getSlotPrefab;
    public GameObject itemDropPrefab;

    Stack<GameObject> itemSlotStack;
    Stack<GameObject> getSlotStack;
    Stack<GameObject> itemDropStack;

    public Transform PoolParent;

    private void Awake()
    {
        itemSlotStack = new Stack<GameObject>();
        getSlotStack = new Stack<GameObject>();
        itemDropStack = new Stack<GameObject>();

        for (int i = 0; i < 200; i++)
        {
            GameObject prefab = Instantiate(itemSlotPrefab, PoolParent.transform);
            prefab.name = $"itemSlot[{i}]";
            itemSlotStack.Push(prefab);
            prefab.SetActive(false);
        }

        for (int i = 0; i < 30; i++)
        {
            GameObject prefab = Instantiate(getSlotPrefab, PoolParent.transform);
            prefab.name = $"getSlot[{i}]";
            getSlotStack.Push(prefab);
            prefab.SetActive(false);
        }

        for (int i = 0; i < 30; i++)
        {
            GameObject prefab = Instantiate(itemDropPrefab, PoolParent.transform);
            prefab.name = $"itemDrop[{i}]";
            itemDropStack.Push(prefab);
            prefab.GetComponent<DropObject>().SetId(i);
            prefab.SetActive(false);
        }

    }
    public ItemSlot Get_ItemSlot()
    {
        ItemSlot itemSlot;
        GameObject prefab;
        if (itemSlotStack.TryPop(out prefab))
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

    public GetSlot Get_GetSlot()
    {
        GetSlot getSlot;
        GameObject prefab;
        if (getSlotStack.TryPop(out prefab))
        {
            getSlot = prefab.GetComponent<GetSlot>();
            return getSlot;
        }
        else
        {
            prefab = Instantiate(getSlotPrefab, PoolParent.transform);
            getSlot = prefab.GetComponent<GetSlot>();
            return getSlot;
        }
    }

    public void Return_itemSlot(ItemSlot itemSlot)
    {
        itemSlotStack.Push(itemSlot.gameObject);
        itemSlot.gameObject.SetActive(false);
        itemSlot.transform.SetParent(PoolParent);
    }


    public void Return_GetSlot(GetSlot getSlot)
    {
        getSlotStack.Push(getSlot.gameObject);
        getSlot.gameObject.SetActive(false);
        getSlot.transform.SetParent(PoolParent);
    }

    public void Return_itemDrop(GameObject itemDrop)
    {
        itemDropStack.Push(itemDrop);
        itemDrop.SetActive(false);
        itemDrop.transform.SetParent(PoolParent);
    }
}
