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
    public Sprite[] characterSprites;
    public Sprite[] chestSprites;
    private void Awake()
    {
        itemDictionary = new Dictionary<int, Item>();
        itemSpriteDictionary = new Dictionary<int, Sprite>();
        characterSpriteDictionary = new Dictionary<Character, Sprite>();

        List<Dictionary<string, object>> data = CSVReader.Read("Genshin_Data");

        for (var i = 0; i < data.Count; i++)
        {
            bool isTrue = data[i]["isEquip"].ToString() == "TRUE" ? true : false;
            DefenceType defenceType;
            switch(data[i]["relicType"].ToString())
            {
                case "Flower":
                    defenceType = DefenceType.Flower;
                    break;
                case "Feather":
                    defenceType = DefenceType.Feather;
                    break;
                case "SandTime":
                    defenceType = DefenceType.SandTime;
                    break;
                case "Trophy":
                    defenceType = DefenceType.Trophy;
                    break;
                case "Crown":
                    defenceType = DefenceType.Crown;
                    break;
                default:
                    defenceType = DefenceType.Flower;
                    break;
            }
            itemDictionary.Add(int.Parse(data[i]["id"].ToString()), new Item(int.Parse(data[i]["id"].ToString()), data[i]["name"].ToString(), int.Parse(data[i]["count"].ToString()), isTrue, float.Parse(data[i]["weaponDamage"].ToString()), defenceType, float.Parse(data[i]["value"].ToString()), data[i]["description"].ToString()));
        }

        int index = 0;
        for(int i = 1; i <= 21; i++)
        {
            itemSpriteDictionary.Add(i, sprites[index++]);
        }
        for (int i = 101; i <= 110; i++)
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

    public Sprite GetCharacterSprite(Character character)
    {
        return characterSpriteDictionary[character];
    }

    public Sprite GetChestSprite(int num)
    {
        return chestSprites[num];
    }

    public DefenceType GetRelicType(int id)
    {
        return itemDictionary[id].defenceType;
    }
}
