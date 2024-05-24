using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    public GameObject itemSlotPrefab;
    public GameObject getSlotPrefab;
    public GameObject itemGetPanelSlotPrefab;
    public GameObject itemDropPrefab;
    public GameObject elementPrefab;
    public GameObject arrowPrefab;
    public GameObject damageTxtPrefab;//damageText

    Stack<GameObject> itemSlotStack;
    Stack<GameObject> getSlotStack;
    Stack<GameObject> itemGetPanelSlotStack;
    Stack<GameObject> itemDropStack;
    Queue<GameObject> elementQueue;
    Queue<GameObject> arrowQueue;
    Queue<GameObject> damageTextQueue;//damageText

    public Transform PoolParent;
    public Transform ElementPool;
    public Transform PlayerTransform;
    public Transform playerCameraTrans;

    private void Awake()
    {
        itemSlotStack = new Stack<GameObject>();
        getSlotStack = new Stack<GameObject>();
        itemGetPanelSlotStack = new Stack<GameObject>();
        itemDropStack = new Stack<GameObject>();
        elementQueue = new Queue<GameObject>();
        arrowQueue = new Queue<GameObject>();
        damageTextQueue = new Queue<GameObject>();

        for (int i = 0; i < 200; i++)
        {
            GameObject prefab = Instantiate(itemSlotPrefab, PoolParent.transform);
            prefab.name = $"itemSlot[{i}]";
            itemSlotStack.Push(prefab);
            prefab.SetActive(false);
        }

        for (int i = 0; i < 15; i++)
        {
            GameObject prefab = Instantiate(getSlotPrefab, PoolParent.transform);
            prefab.name = $"getSlot[{i}]";
            getSlotStack.Push(prefab);
            prefab.SetActive(false);
        }

        for (int i = 0; i < 15; i++)
        {
            GameObject prefab = Instantiate(itemGetPanelSlotPrefab, PoolParent.transform);
            prefab.name = $"itemGetPanelSlot[{i}]";
            itemGetPanelSlotStack.Push(prefab);
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

        for (int i = 0; i < 20; i++)
        {
            GameObject prefab = Instantiate(arrowPrefab, PoolParent.transform);
            arrowQueue.Enqueue(prefab);
            prefab.SetActive(false);
        }

        //damageText복제
        for (int i = 0; i < 30; i++)
        {
           GameObject prefab = Instantiate(damageTxtPrefab, PoolParent.transform);
           damageTextQueue.Enqueue(prefab);
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

    public DropObject Get_DropObject(int id)
    {
        DropObject dropObject;
        GameObject prefab;
        if (itemDropStack.TryPop(out prefab))
        {
            dropObject = prefab.GetComponent<DropObject>();
            dropObject.SetItem(id);
            dropObject.gameObject.SetActive(true);
            return dropObject;
        }
        else
        {
            prefab = Instantiate(itemDropPrefab, PoolParent.transform);
            dropObject = prefab.GetComponent<DropObject>();
            dropObject.SetItem(id);
            return dropObject;
        }
    }

    public ItemGetPanelSlot Get_ItemGetPanelSlot()
    {
        ItemGetPanelSlot itemGetPanelSlot;
        GameObject prefab;
        if (itemGetPanelSlotStack.TryPop(out prefab))
        {
            itemGetPanelSlot = prefab.GetComponent<ItemGetPanelSlot>();
            return itemGetPanelSlot;
        }
        else
        {
            prefab = Instantiate(itemGetPanelSlotPrefab, PoolParent.transform);
            itemGetPanelSlot = prefab.GetComponent<ItemGetPanelSlot>();
            return itemGetPanelSlot;
        }
    }

    public GameObject Get_Arrow()
    {
        GameObject arrow;
        if(arrowQueue.Count > 0)
        {
            arrow = arrowQueue.Dequeue();
        }
        else
        {
            arrow = Instantiate(arrowPrefab, PoolParent.transform);
        }

        arrowQueue.Enqueue(arrow);
        arrow.transform.GetChild(0).localPosition = Vector3.zero;
        arrow.transform.localPosition = Vector3.zero;
        arrow.GetComponent<Rigidbody>().velocity = Vector3.zero;
        arrow.SetActive(false);
        return arrow;
    }

    //damageText
    public void Get_Text(float damage , Vector3 monsterPos)
    {
        GameObject text = damageTextQueue.Dequeue();//제거
        damageTextQueue.Enqueue(text);//넣기
        text.SetActive(true);
        text.transform.position = monsterPos + Vector3.up * 1.5f;                             
        text.GetComponentInChildren<TextMeshProUGUI>().text = damage.ToString();//데미지 숫자 넣기
        text.GetComponent<DamageText>().SetCameraTrans(playerCameraTrans);//카메라 바라보기 , 5초뒤 꺼짐
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

    public void Return_ItemGetPanelSlot(ItemGetPanelSlot itemGetPanelSlot)
    {
        itemGetPanelSlotStack.Push(itemGetPanelSlot.gameObject);
        itemGetPanelSlot.gameObject.SetActive(false);
        itemGetPanelSlot.transform.SetParent(PoolParent);
        UIManager.Instance.Check_GetSlot_J();       
    }

    public void Return_itemDrop(GameObject itemDrop)
    {
        itemDropStack.Push(itemDrop);
        itemDrop.SetActive(false);
        itemDrop.transform.SetParent(PoolParent);
    }

    public void Return_Arrow(GameObject arrow)
    {
        arrowQueue.Enqueue(arrow);
        arrow.SetActive(false);
        arrow.transform.SetParent(PoolParent);
    }
}