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
    public void Load_Weapon(Transform weaponContent)
    {
        foreach(ItemSlot itemSlot in slotWeaponDictionary.Values)
        {
            switch( EquipManager.Instance.character)
            {
                case CharacterItemSprite.Beidou:
                    if (itemSlot.GetEquipType() != EqiupType.Claymore) continue;
                    break;
                case CharacterItemSprite.Kokomi:
                    if (itemSlot.GetEquipType() != EqiupType.Catalyst) continue;
                    break;
                case CharacterItemSprite.Wriothesley:
                    if (itemSlot.GetEquipType() != EqiupType.Catalyst) continue;
                    break;
                case CharacterItemSprite.Yoimiya:
                    if (itemSlot.GetEquipType() != EqiupType.Bow) continue;
                    break;
            }
            itemSlot.transform.SetParent(weaponContent);
            itemSlot.transform.localScale = Vector3.one;

            show_Slots.Add(itemSlot);
        }
    }

    public void Load_Relic(Transform relicContent, EqiupType equipType)
    {
        foreach (ItemSlot itemSlot in slotdefenceDictionary.Values)
        {
            if (itemSlot.GetEquipType() != equipType) continue;
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
        else if((int)item.equipType < 5)
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

    public void InitItemSet()
    {
        //무기
        {
            GetItem(ItemDatabase.Instance.GetItem(5));
            GetItem(ItemDatabase.Instance.GetItem(10));
            GetItem(ItemDatabase.Instance.GetItem(9));
            GetItem(ItemDatabase.Instance.GetItem(6));
            GetItem(ItemDatabase.Instance.GetItem(13));
        }

        //성유물
        for (int i = 0; i < 3; i++)
        {
            GetItem(ItemDatabase.Instance.GetItem(101));
            GetItem(ItemDatabase.Instance.GetItem(102));
            GetItem(ItemDatabase.Instance.GetItem(103));
            GetItem(ItemDatabase.Instance.GetItem(104));
            GetItem(ItemDatabase.Instance.GetItem(105));
        }
        for (int i = 0; i < 1; i++)
        {
            GetItem(ItemDatabase.Instance.GetItem(106));
            GetItem(ItemDatabase.Instance.GetItem(107));
            GetItem(ItemDatabase.Instance.GetItem(108));
            GetItem(ItemDatabase.Instance.GetItem(109));
            GetItem(ItemDatabase.Instance.GetItem(110));
        }
        for (int i = 0; i < 1; i++)
        {
            GetItem(ItemDatabase.Instance.GetItem(111));
            GetItem(ItemDatabase.Instance.GetItem(112));
            GetItem(ItemDatabase.Instance.GetItem(113));
            GetItem(ItemDatabase.Instance.GetItem(114));
            GetItem(ItemDatabase.Instance.GetItem(115));
        }
        //초상화 적용
        for (int i = 0;i < 4;i++)
        {
            slotWeaponDictionary[i].OwnerChange((CharacterItemSprite)i);
            slotdefenceDictionary[0 +5*i].OwnerChange((CharacterItemSprite)i);            
            slotdefenceDictionary[1 +5*i].OwnerChange((CharacterItemSprite)i);
            slotdefenceDictionary[2 +5*i].OwnerChange((CharacterItemSprite)i);
            slotdefenceDictionary[3 +5*i].OwnerChange((CharacterItemSprite)i);
            slotdefenceDictionary[4 +5*i].OwnerChange((CharacterItemSprite)i);

            //장비착용
            EquipManager.Instance.Equip(slotWeaponDictionary[i]);
            EquipManager.Instance.Equip(slotdefenceDictionary[0 + 5 * i]);
            EquipManager.Instance.Equip(slotdefenceDictionary[1 + 5 * i]);
            EquipManager.Instance.Equip(slotdefenceDictionary[2 + 5 * i]);
            EquipManager.Instance.Equip(slotdefenceDictionary[3 + 5 * i]);
            EquipManager.Instance.Equip(slotdefenceDictionary[4 + 5 * i]);
        }
        GetItem(ItemDatabase.Instance.GetItem(8));

        Item item = ItemDatabase.Instance.GetItem(1010);
        item.count = 1000;
        GetItem(item);
    }

    public ItemSlot GetWeaponItemSlot(int key)
    {
        return slotWeaponDictionary[key];
    }
    public ItemSlot GetRelicItemSlot(int key)
    {
        return slotdefenceDictionary[key];
    }
    public Item GetWeaponItem(int key)
    {
        return weaponDictionary[key];
    }
    public Item GetRelicItem(int key)
    {
        return defenceDictionary[key];
    }
}
