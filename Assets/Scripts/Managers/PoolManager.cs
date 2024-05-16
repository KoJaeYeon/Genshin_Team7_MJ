using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    public GameObject itemSlotPrefab;
    public GameObject getSlotPrefab;
    public GameObject itemDropPrefab;
    public GameObject elementPrefab;

    Stack<GameObject> itemSlotStack;
    Stack<GameObject> getSlotStack;
    Stack<GameObject> itemDropStack;
    Queue<GameObject> elementQueue;

    public Transform PoolParent;
    public Transform ElementPool;
    public Transform PlayerTransform;

    private void Awake()
    {
        itemSlotStack = new Stack<GameObject>();
        getSlotStack = new Stack<GameObject>();
        itemDropStack = new Stack<GameObject>();
        elementQueue = new Queue<GameObject>();

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

        for (int i = 0; i < 100; i++)
        {
            GameObject prefab = Instantiate(elementPrefab, ElementPool);
            ElementObject element = prefab.GetComponent<ElementObject>();
            element.SetPlayerTransform(PlayerTransform);
            prefab.SetActive(false);
            elementQueue.Enqueue(prefab);
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

    public GameObject GetElementObject()
    {
        GameObject prefab = elementQueue.Dequeue();
        elementQueue.Enqueue(prefab);
        return prefab;
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