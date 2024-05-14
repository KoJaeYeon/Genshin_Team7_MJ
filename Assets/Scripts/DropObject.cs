using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObject : MonoBehaviour,IInteractable
{
    Item item;
    GetSlot getSlot;

    public void InitItemSlot()
    {
       // getSlot.Init(item);
    }
    public void RemoveItemGet()
    {
        Debug.Log("RemoveGet");
    }

    public void SetItem(Item item)
    {
        this.item = item;
    }

    public void UpdateItemGet()
    {
        Debug.Log("UpdateItemGet");
    }

    public void UseItemGet()
    {
        Debug.Log("UseItemGET");
    }
}
