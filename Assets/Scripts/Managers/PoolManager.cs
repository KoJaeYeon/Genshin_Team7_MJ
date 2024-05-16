using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    public GameObject itemSlotPrefab;
    public GameObject itemDropPrefab;
    public GameObject elementPrefab;

    Stack<GameObject> itemSlotStack;
    Stack<GameObject> itemDropStack;
    Queue<GameObject> elementQueue;

    public Transform PoolParent;
    public Transform ElementPool;
    public Transform PlayerTransform;

    private void Awake()
    {
        itemSlotStack = new Stack<GameObject>();
        elementQueue = new Queue<GameObject>();

        for(int i = 0; i < 200; i++)
        {
            GameObject prefab = Instantiate(itemSlotPrefab,PoolParent.transform);
            prefab.name = i.ToString();
            itemSlotStack.Push(prefab);
            prefab.SetActive(false);
        }

        for(int i = 0; i < 100; i++)
        {
            GameObject prefab = Instantiate(elementPrefab, ElementPool);
            ElementObject element = prefab.GetComponent<ElementObject>();
            element.SetPlayerTransform(PlayerTransform);
            prefab.SetActive(false);
            elementQueue.Enqueue(prefab);
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

    public GameObject GetElementObject()
    {
        GameObject prefab = elementQueue.Dequeue();
        elementQueue.Enqueue(prefab);
        return prefab;
    }
}
