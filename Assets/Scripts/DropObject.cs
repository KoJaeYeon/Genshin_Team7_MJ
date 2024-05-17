using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DropObject : MonoBehaviour,IInteractable
{
    Item item;
    GetSlot getSlot;
    //아이템패널 클래스랑 변수선언해서 받아오기 GetSlot처럼
    ItemGetPanelSlot getpanelSlot;
    int id;
    static int idtemp = 1;
    private void Start()
    {
        getSlot = PoolManager.Instance.Get_GetSlot();
        getpanelSlot = PoolManager.Instance.Get_ItemGetPanelSlot();

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
        //초기화해주기 해당아이템으로
       getpanelSlot.Init_J(item);
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
        // 여기다가 호출
        UIManager.Instance.AddGetSlot_J(getpanelSlot);
        getpanelSlot.gameObject.SetActive(true);


       PoolManager.Instance.Return_GetSlot(getSlot);
        PoolManager.Instance.Return_itemDrop(gameObject);

        Invoke("Destroy", 1);
    }

    public void Destroy()
    {

        PoolManager.Instance.Return_ItemGetPanelSlot(getpanelSlot);
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
