using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : Singleton<ItemDatabase>
{
    Dictionary<ItemName, Item> itemDictionary = new Dictionary<ItemName, Item>();
    Dictionary<ItemName, Sprite> itemSpriteDictionary = new Dictionary<ItemName, Sprite>();
    private void Awake()
    {        
        itemDictionary.Add(ItemName.ClayMore1, new Item(ItemName.ClayMore1, 1, true, 70, DefenceType.Head, 0));
        itemDictionary.Add(ItemName.ClayMore2, new Item(ItemName.ClayMore2, 1, true, 60, DefenceType.Head, 0));
    }

    public Item GetItem(ItemName itemName)
    {
        return itemDictionary[itemName];
    }

    public Sprite GetSprite(ItemName itemName)
    {
        return itemSpriteDictionary[itemName];
    }
}
