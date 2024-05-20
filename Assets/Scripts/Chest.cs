using System;
using System.Collections;
using UnityEngine;

public enum ChestType
{
    Jungyo,
    Jingui
}
public class Chest : MonoBehaviour,IInteractable
{
    DropObject[] dropObjects;
    GetSlot getSlot;
    static int idtemp = 1;
    public int[] keys;
    public ChestType chestType;
    int id;
    private void Start()
    {
        dropObjects = new DropObject[keys.Length];

        getSlot = PoolManager.Instance.Get_GetSlot();
        for (int i = 0;  i < keys.Length; i++)
        {
            dropObjects[i] = PoolManager.Instance.Get_DropObject(keys[i]);
        }
        InitItemSlot();

    }

    public void InitItemSlot()
    {
        getSlot.Init(chestType);
    }

    public void SetId(int id)
    {
        this.id = id;
    }
    public void UpdateItemGet()
    {
        Debug.Log("UpdateItemGet_Chest");
        UIManager.Instance.AddGetSlot(getSlot);
        getSlot.gameObject.SetActive(true);
    }

    public void UseItemGet()
    {
        Debug.Log("UseItem_Chest");
        PoolManager.Instance.Return_GetSlot(getSlot);
        foreach(DropObject dropObject in dropObjects)
        {
            dropObject.gameObject.SetActive(true);
            dropObject.transform.position = transform.position;
        }
        StartCoroutine(DisappearChest());
    }

    IEnumerator DisappearChest()
    {
        Animator animator = GetComponent<Animator>();
        animator.Play("Chest_Open");
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.enabled = false;
        yield return new WaitForSeconds(2f);
        //animator.Play("Chest_Disppear");
        yield return null;
        collider.enabled = true;
        gameObject.SetActive(false);

    }
    public void RemoveItemGet()
    {
        Debug.Log("RemoveGet_Chest");
        UIManager.Instance.RemoveGetSlot();
        getSlot.transform.SetParent(PoolManager.Instance.PoolParent);
        getSlot.gameObject.SetActive(false);
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        if (!(obj is Chest)) return false;
        Chest other = (Chest)obj;
        if(other.id == id) return true;
        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
