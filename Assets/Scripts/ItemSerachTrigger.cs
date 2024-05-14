using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSerachTrigger : MonoBehaviour
{
    List<IInteractable> items;
    int searchPoint = 0;
    int itemCount;
    public void UpSearchPoint()
    {
        if(searchPoint < items.Count - 1)
        {
            searchPoint++;
            UIManager.Instance.SetFPoint(searchPoint);
        }
    }

    public void DownSearchPoint()
    {
        if(searchPoint > 0)
        {
            searchPoint--;
            UIManager.Instance.SetFPoint(searchPoint);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y)) UpSearchPoint();
        else if (Input.GetKeyDown(KeyCode.H)) DownSearchPoint();
        else if (Input.GetKeyDown(KeyCode.F))
        {
            items[searchPoint].UseItemGet();
            items.RemoveAt(searchPoint);
            if (searchPoint > 0) searchPoint--;
            UIManager.Instance.SetFPoint(searchPoint);
        }
        itemCount = items.Count;
    }

    private void Awake()
    {
        items = new List<IInteractable>();
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
