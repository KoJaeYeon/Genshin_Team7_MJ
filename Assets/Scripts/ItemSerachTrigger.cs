using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSerachTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.gameObject.GetComponent<IInteractable>();
        if(interactable != null )
        {
            interactable.UpdateItemGet();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.gameObject.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactable.RemoveItemGet();
        }
    }
}
