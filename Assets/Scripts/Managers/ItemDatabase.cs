using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterItemSprite
{
    Beidou,
    Kokomi,
    Wriothesley,
    Yoimiya,
    None
}
public class ItemDatabase : Singleton<ItemDatabase>
{
    Dictionary<int, Item> itemDictionary;
    Dictionary<int, Sprite> itemSpriteDictionary;
    Dictionary<CharacterItemSprite, Sprite> characterSpriteDictionary;

    public Sprite[] sprites;
    public Sprite[] characterSprites;
    public Sprite[] chestSprites;
    private void Awake()
    {
        itemDictionary = new Dictionary<int, Item>();
        itemSpriteDictionary = new Dictionary<int, Sprite>();
        characterSpriteDictionary = new Dictionary<CharacterItemSprite, Sprite>();

        List<Dictionary<string, object>> data = CSVReader.Read("Genshin_Data");

        for (var i = 0; i < data.Count; i++)
        {
            bool isTrue = data[i]["isEquip"].ToString() == "TRUE" ? true : false;
            EqiupType eqiupType;
            switch(data[i]["equipType"].ToString())
            {
                case "Flower":
                    eqiupType = EqiupType.Flower;
                    break;
                case "Feather":
                    eqiupType = EqiupType.Feather;
                    break;
                case "SandTime":
                    eqiupType = EqiupType.SandTime;
                    break;
                case "Trophy":
                    eqiupType = EqiupType.Trophy;
                    break;
                case "Crown":
                    eqiupType = EqiupType.Crown;
                    break;
                default:
                    eqiupType = EqiupType.Flower;
                    break;
            }
            itemDictionary.Add(int.Parse(data[i]["id"].ToString()), new Item(int.Parse(data[i]["id"].ToString()), data[i]["name"].ToString(), int.Parse(data[i]["count"].ToString()), isTrue, eqiupType, float.Parse(data[i]["value"].ToString()), data[i]["description"].ToString()));
        }

        for(int i = 0; i < characterSprites.Length; i++)
        {
            characterSpriteDictionary.Add((CharacterItemSprite)i, characterSprites[i]);
        }

        int index = 0;
        for(int i = 1; i <= 21; i++)
        {
            itemSpriteDictionary.Add(i, sprites[index++]);
        }
        for (int i = 101; i <= 115; i++)
        {
            itemSpriteDictionary.Add(i, sprites[index++]);
        }
        for (int i = 1001; i <= 1009; i++)
        {
            itemSpriteDictionary.Add(i, sprites[index++]);
        }
    }

    public Item GetItem(int id)
    {
        return new Item(itemDictionary[id]);
    }

    public Sprite GetItemSprite(int id)
    {
        return itemSpriteDictionary[id];
    }

    public Sprite GetCharacterSprite(CharacterItemSprite character)
    {
        return characterSpriteDictionary[character];
    }

    public Sprite GetChestSprite(int num)
    {
        return chestSprites[num];
    }

    public EqiupType GetRelicType(int id)
    {
        return itemDictionary[id].equipType;
    }
}
