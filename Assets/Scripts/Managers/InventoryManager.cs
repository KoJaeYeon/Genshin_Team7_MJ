using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    Dictionary<int, Item> weaponDictionary;
    Dictionary<int, Item> defenceDictionary;
    Dictionary<int, Item> otherDictionary;

    Dictionary<int, ItemSlot> slotWeaponDictionary;
    Dictionary<int, ItemSlot> slotdefenceDictionary;
    Dictionary<int, ItemSlot> slototherDictionary;

    public Transform weaponTrans;
    public Transform defenceTrans;
    public Transform otherTrans;

    List<ItemSlot> show_Slots;

    private void Awake()
    {
        weaponDictionary = new Dictionary<int, Item>();
        defenceDictionary = new Dictionary<int, Item>();
        otherDictionary = new Dictionary<int, Item>();

        slotWeaponDictionary = new Dictionary<int, ItemSlot>();
        slotdefenceDictionary = new Dictionary<int, ItemSlot>();
        slototherDictionary = new Dictionary<int, ItemSlot>();
        
        show_Slots = new List<ItemSlot>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GetItem(ItemDatabase.Instance.GetItem(1));
            Debug.Log("GetItem");
        }
        else if (Input.GetKeyDown(KeyCode.F1))
        {
            GetItem(ItemDatabase.Instance.GetItem(21));
            Debug.Log("GetItem");
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            GetItem(ItemDatabase.Instance.GetItem(101));
            Debug.Log("GetItem");
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            GetItem(ItemDatabase.Instance.GetItem(1001));
            Debug.Log("GetItem");
        }
    }

    public void Load_Weapon(Transform weaponContent)
    {
        foreach(ItemSlot itemSlot in slotWeaponDictionary.Values)
        {
            itemSlot.transform.SetParent(weaponContent);
            itemSlot.transform.localScale = Vector3.one;

            show_Slots.Add(itemSlot);
        }
    }

    public void Load_Relic(Transform relicContent, DefenceType defenceType)
    {
        foreach (ItemSlot itemSlot in slotdefenceDictionary.Values)
        {
            if (itemSlot.GetRelicType() != defenceType) continue;
            itemSlot.transform.SetParent(relicContent);
            itemSlot.transform.localScale = Vector3.one;

            show_Slots.Add(itemSlot);
        }
    }

    public void UnLoad_Weapon()
    {
        while(show_Slots.Count > 0)
        {
            show_Slots[0].transform.SetParent(weaponTrans);
            show_Slots.RemoveAt(0);
        }
    }

    public void UnLoad_Relic()
    {
        while (show_Slots.Count > 0)
        {
            show_Slots[0].transform.SetParent(defenceTrans);
            show_Slots.RemoveAt(0);
        }
    }

    public void GetItem(Item item)
    {
        if (item.isEquip == false)
        {
            if (otherDictionary.TryAdd(item.id, item))
            {
                SetOtherSlot(item.id, otherDictionary[item.id]);
            }
            else
            {
                otherDictionary[item.id].count += item.count;
                SetOtherSlot(item.id, otherDictionary[item.id]);
            }
        }
        else if(item.weaponDamage > 0)
        {
            int index = 0;
            while(weaponDictionary.ContainsKey(index))
            {
                index++;
            }
            weaponDictionary.Add(index, item);
            SetWeaponSlot(index,item);
        }
        else
        {
            int index = 0;
            while (defenceDictionary.ContainsKey(index))
            {
                index++;
            }
            defenceDictionary.Add(index, item);
            SetDefenceSlot(index, item);
        }
    }

    public void SetWeaponSlot(int key,Item item)
    {
        if (slotWeaponDictionary.ContainsKey(key))
        {
            slotWeaponDictionary[key].UpdateSlot(item);
        }
        else
        {
            ItemSlot itemSlot = PoolManager.Instance.Get_ItemSlot();
            itemSlot.transform.SetParent(weaponTrans);
            itemSlot.transform.localScale = Vector3.one;
            slotWeaponDictionary.Add(key, itemSlot);
            itemSlot.InitUpdateSlot(key,item);
            itemSlot.gameObject.SetActive(true);
        }
    }
    public void SetDefenceSlot(int key, Item item)
    {
        if (slotdefenceDictionary.ContainsKey(key))
        {
            slotdefenceDictionary[key].UpdateSlot(item);
        }
        else
        {
            ItemSlot itemSlot = PoolManager.Instance.Get_ItemSlot();
            itemSlot.transform.SetParent(defenceTrans);
            itemSlot.transform.localScale = Vector3.one;
            slotdefenceDictionary.Add(key, itemSlot);
            itemSlot.InitUpdateSlot(key, item);
            itemSlot.gameObject.SetActive(true);
        }
    }
    public void SetOtherSlot(int key, Item item)
    {
        if (slototherDictionary.ContainsKey(key))
        {
            Debug.Log("other");
            slototherDictionary[key].UpdateSlot(item);
        }
        else
        {
            ItemSlot itemSlot = PoolManager.Instance.Get_ItemSlot();
            itemSlot.transform.SetParent(otherTrans);
            itemSlot.transform.localScale = Vector3.one;
            slototherDictionary.Add(key, itemSlot);
            itemSlot.InitUpdateSlot(key, item);
            itemSlot.gameObject.SetActive(true);
        }
    }
}
