using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    int key;
    int id;
    string name;
    int count;
    Image itemImage;
    Image characterImage;

    private void Awake()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
        characterImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }
    public void InitUpdateSlot(int key, Item item)
    {
        this.key = key;
        this.id = item.id;
        this.name = item.id.ToString();
        this.count = item.count;
        this.itemImage.sprite = ItemDatabase.Instance.GetItemSprite(id);
    }
    public void UpdateSlot(Item item)
    {
        this.count = item.count;
    }

    public void OwnerChange(Character character)
    {

    }
}
