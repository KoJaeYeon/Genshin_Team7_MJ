using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DropObject : MonoBehaviour,IInteractable
{
    Item item;
    GetSlot getSlot;
    ItemGetPanelSlot getpanelSlot;
    static int idtemp = 1;
    int id; //���������Ʈ �񱳸� ���� ���� id

    public void InitItemSlot()
    {
        getSlot = PoolManager.Instance.Get_GetSlot();
        getpanelSlot = PoolManager.Instance.Get_ItemGetPanelSlot();

        getSlot.Init(item);
        getpanelSlot.Init_J(item);
    }

    public void SetItem(int id) // ��� ������Ʈ�� �������� �������ִ� �Լ�
    {
        item = ItemDatabase.Instance.GetItem(id);
        InitItemSlot();
    }

    public void SetItemCount(int count)
    {
        this.item.count = count;
        UpdateItemSlot();
    }

    private void UpdateItemSlot()
    {
        getSlot.Init(item);
        getpanelSlot.Init_J(item);
    }

    public void SetId(int id) // ��� ������Ʈ�� ���̵� �������ִ� �Լ�
    {
        this.id = id;
    }


    public void RemoveItemGet()
    {
        Debug.Log("RemoveGet");
        UIManager.Instance.RemoveGetSlot();
        getSlot.transform.SetParent(PoolManager.Instance.PoolParent);
        getSlot.gameObject.SetActive(false);
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
        UIManager.Instance.AddGetSlot_J(getpanelSlot);
        getpanelSlot.gameObject.SetActive(true);


        PoolManager.Instance.Return_GetSlot(getSlot);
        PoolManager.Instance.Return_itemDrop(gameObject);

        getpanelSlot.Destroy();
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
