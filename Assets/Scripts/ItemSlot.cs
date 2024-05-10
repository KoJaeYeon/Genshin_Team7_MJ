using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    int key;
    ItemName itemName;
    string name;
    int count;
    Image itemImage;
    Image charahcterImage;
    public void InitUpdateSlot(int key, Item item)
    {
        this.key = key;
        this.itemName = item.itemName;
        this.name = item.itemName.ToString();
        this.count = item.count;
        this.itemImage.sprite = ItemDatabase.Instance.GetItemSprite(itemName);
    }
    public void UpdateSlot(Item item)
    {
        this.count = item.count;
    }

    public void OwnerChange(Character character)
    {

    }
}
