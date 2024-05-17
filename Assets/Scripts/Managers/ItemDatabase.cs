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

        itemDictionary.Add(1, new Item(1, "진주를 문 해황", 1, true, 70, DefenceType.Flower, 0));
        itemDictionary.Add(2, new Item(2, "성배", 1, true, 0, DefenceType.Trophy, 10));
        itemDictionary.Add(3, new Item(3, "여행자1", 1, false, 0, DefenceType.Flower, 0));
        itemDictionary.Add(4, new Item(4, "여행자2", 1, false, 0, DefenceType.Flower, 0));

        itemSpriteDictionary.Add(1, sprites[0]);
        itemSpriteDictionary.Add(2, sprites[1]);
        itemSpriteDictionary.Add(3, sprites[2]);
        itemSpriteDictionary.Add(4, sprites[3]);
    }

    public Item GetItem(int id)
    {
        return new Item(itemDictionary[id]);
    }

    public Sprite GetItemSprite(int id)
    {
        return itemSpriteDictionary[id];
    }

    public Sprite GetCharacterSprite(Character character)
    {
        return characterSpriteDictionary[character];
    }

    public DefenceType GetRelicType(int id)
    {
        return itemDictionary[id].defenceType;
    }
}
