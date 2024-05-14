using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSerachTrigger : MonoBehaviour
{
    List<IInteractable> items;
    int searchPoint = 0;

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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.gameObject.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactable.RemoveItemGet();
            items.Remove(interactable);
        }
    }
}
