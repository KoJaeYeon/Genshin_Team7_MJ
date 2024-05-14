using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DropObject : MonoBehaviour,IInteractable
{
    Item item;
    GetSlot getSlot;
    int id;
    static int idtemp = 1;
    private void Start()
    {
        getSlot = PoolManager.Instance.Get_GetSlot();

        //삭제예정
        item = ItemDatabase.Instance.GetItem(idtemp++);

        InitItemSlot();
    }

    public void SetId(int id)
    {
        this.id = id;
    }

    public void InitItemSlot()
    {
        getSlot.Init(item);
    }
    public void RemoveItemGet()
    {
        Debug.Log("RemoveGet");
        UIManager.Instance.RemoveGetSlot();
        getSlot.transform.SetParent(PoolManager.Instance.PoolParent);
        getSlot.gameObject.SetActive(false);
    }

    public void SetItem(Item item)
    {
        this.item = item;
    }

    public void UpdateItemGet()
    {
        Debug.Log("UpdateItemGet");
        UIManager.Instance.AddGetSlot(getSlot);
        getSlot.gameObject.SetActive(true);
    }

    public void UseItemGet()
    {
        InventoryManager.Instance.GetItem(item);
        PoolManager.Instance.Return_GetSlot(getSlot);
        PoolManager.Instance.Return_itemDrop(gameObject);
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        if (!(obj is DropObject)) return false;
        DropObject other = (DropObject)obj;
        if(other.id == id) return true;
        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
