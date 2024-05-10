using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    Dictionary<int, Item> weaponDictionary;
    Dictionary<int, Item> defenceDictionary;
    Dictionary<int, Item> otherDictionary;

    Dictionary<int, ItemSlot> slotWeaponDictionary;
    Dictionary<int, ItemSlot> slotdefenceDictionary;
    Dictionary<int, ItemSlot> slototherDictionary;

    private void Awake()
    {
        weaponDictionary = new Dictionary<int, Item>();
        defenceDictionary = new Dictionary<int, Item>();
        otherDictionary = new Dictionary<int, Item>();

        slotWeaponDictionary = new Dictionary<int, ItemSlot>();
        slotdefenceDictionary = new Dictionary<int, ItemSlot>();
        slototherDictionary = new Dictionary<int, ItemSlot>();

        
    }

    public void GetItem(Item item)
    {
        if (item.isEquip == false)
        {
            if (otherDictionary.TryAdd((int)item.itemName, item))
            {
                otherDictionary[(int)item.itemName].count += item.count;
                SetOtherSlot((int)item.itemName, otherDictionary[(int)item.itemName]);
            }
        }
        else if(item.weaponDamage > 0)
        {
            weaponDictionary.Add(weaponDictionary.Count, item);
            SetWeaponSlot(weaponDictionary.Count-1,item);
        }
        else
        {
            defenceDictionary.Add(defenceDictionary.Count, item);
            SetDefenceSlot(weaponDictionary.Count-1, item);
        }
    }
    public void SetWeaponSlot(int key,Item item)
    {
        if (weaponDictionary.ContainsKey(key))
        {
            slotWeaponDictionary[key].UpdateSlot(item);
        }
        else
        {
            ItemSlot itemSlot = PoolManager.Instance.GetItemSlot();
            slotWeaponDictionary.Add(key, itemSlot);
            itemSlot.InitUpdateSlot(key,item);
        }
    }
    public void SetDefenceSlot(int key, Item item)
    {
        if (defenceDictionary.ContainsKey(key))
        {
            slotdefenceDictionary[key].UpdateSlot(item);
        }
        else
        {
            ItemSlot itemSlot = PoolManager.Instance.GetItemSlot();
            slotdefenceDictionary.Add(key, itemSlot);
            itemSlot.InitUpdateSlot(key,item);
        }
    }
    public void SetOtherSlot(int key, Item item)
    {
        if(otherDictionary.ContainsKey(key))
        {
            slototherDictionary[key].UpdateSlot(item);
        }
        else
        {
            ItemSlot itemSlot = PoolManager.Instance.GetItemSlot();
            slototherDictionary.Add(key, itemSlot);
            itemSlot.InitUpdateSlot(key,item);
        }
    }


}
