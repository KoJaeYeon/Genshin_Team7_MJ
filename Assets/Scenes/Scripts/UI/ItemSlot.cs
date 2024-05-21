using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    int key;
    int id;
    string itemName;
    int count;
    CharacterItemSprite character;
    Image itemImage;
    Image characterImage;
    TextMeshProUGUI text;

    private void Awake()
    {
        itemImage = transform.GetChild(0). GetChild(0).GetComponent<Image>();
        characterImage = transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>();
        text = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    public void InitUpdateSlot(int key, Item item)
    {
        this.key = key;
        this.id = item.id;
        this.itemName = item.itemName;
        this.count = item.count;
        this.itemImage.sprite = ItemDatabase.Instance.GetItemSprite(id);
        if (item.isEquip == false)
        {
            text.text = item.count.ToString();
        }
        else if (item.weaponDamage > 0)
        {
            text.text = "Lv.90";
        }
        else
        {
            text.text = "+20";
        }
        characterImage.transform.parent.gameObject.SetActive(false);
    }
    public void UpdateSlot(Item item)
    {
        this.count = item.count;
        text.text = item.count.ToString();
    }

    public void ShowData()
    {
        UIManager.Instance.showDataUpdate(id);
    }

    public void OwnerChange(CharacterItemSprite character)
    {
        switch(character)
        {
            case CharacterItemSprite.None:
                characterImage.transform.parent.gameObject.SetActive(false);
                break;
            default:
                this.character = character;
                characterImage.sprite = ItemDatabase.Instance.GetCharacterSprite(character);
                characterImage.transform.parent.gameObject.SetActive(true);
                break;
        }        
    }

    public DefenceType GetRelicType()
    {
        return ItemDatabase.Instance.GetRelicType(id);
    }
}
