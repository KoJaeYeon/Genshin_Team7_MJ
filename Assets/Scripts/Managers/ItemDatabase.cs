using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Character
{
    None,
    Yoimiya,
    Beidu
}
public class ItemDatabase : Singleton<ItemDatabase>
{
    Dictionary<int, Item> itemDictionary;
    Dictionary<int, Sprite> itemSpriteDictionary;
    Dictionary<Character, Sprite> characterSpriteDictionary;

    public Sprite[] sprites;
    private void Awake()
    {
        itemDictionary = new Dictionary<int, Item>();
        itemSpriteDictionary = new Dictionary<int, Sprite>();
        characterSpriteDictionary = new Dictionary<Character, Sprite>();

        itemDictionary.Add(1, new Item(1, 1, true, 70, DefenceType.Head, 0));
        itemDictionary.Add(2, new Item(2, 1, true, 60, DefenceType.Head, 0));
        
        itemSpriteDictionary.Add(1, sprites[0]);
        itemSpriteDictionary.Add(2, sprites[1]);
    }

    public Item GetItem(int id)
    {
        return itemDictionary[id];
    }

    public Sprite GetItemSprite(int id)
    {
        Debug.Log("sp" + id);
        Debug.Log(itemSpriteDictionary[id]);
        return itemSpriteDictionary[id];
    }

    public Sprite GetCharacterSprite(Character character)
    {
        return characterSpriteDictionary[character];
    }
}
