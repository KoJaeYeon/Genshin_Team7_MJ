using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSerachTrigger : Singleton<ItemSerachTrigger>
{
    List<IInteractable> items;
    int searchPoint = 0;
    int itemCount;
    private void Awake()
    {
        items = new List<IInteractable>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            GetItem();
            itemCount = items.Count;
        }
    }

    public bool SearchPoint(float value)
    {
        if (value > 0)
        {
            UpSearchPoint();
        }
        else if (value < 0)
        {
            DownSearchPoint();
        }

        return items.Count > 1;
    }
    public void UpSearchPoint()
    {
        if (searchPoint < items.Count - 1)
        {
            searchPoint++;
            UIManager.Instance.SetFPoint(searchPoint);
        }
    }

    public void DownSearchPoint()
    {
        if (searchPoint > 0)
        {
            searchPoint--;
            UIManager.Instance.SetFPoint(searchPoint);
        }
    }

    private void GetItem()
    {
        if (items.Count == 0) return;
        items[searchPoint].UseItemGet();
        items.RemoveAt(searchPoint);
        if (searchPoint > 0) searchPoint--;
        UIManager.Instance.SetFPoint(searchPoint);
        itemCount = items.Count;
    }

    /// <summary>
    /// 모바일 사용성을 위한 버튼 호출함수
    /// </summary>
    public void GetItemIndex(int index)
    {
        searchPoint = items.Count -1 - index;
        GetItem();
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.gameObject.GetComponent<IInteractable>();
        if(interactable != null )
        {
            interactable.UpdateItemGet();
            items.Add(interactable);
            UIManager.Instance.SetFPoint(searchPoint);            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.gameObject.GetComponent<IInteractable>();
        if (interactable != null)
        {
            int index = items.IndexOf(interactable);
            interactable.RemoveItemGet();
            items.Remove(interactable);
            if (index <= searchPoint) searchPoint--;
            if (searchPoint < 0) searchPoint = 0;
            UIManager.Instance.SetFPoint(searchPoint);
        }
    }
}
