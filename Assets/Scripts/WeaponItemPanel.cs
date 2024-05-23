using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponItemPanel : MonoBehaviour,IItemPanel
{
    TextMeshProUGUI itemName;
    TextMeshProUGUI itemType;
    TextMeshProUGUI value;

    Image equipCharacter;
    TextMeshProUGUI equipText;

    GameObject equipButton;
    private void Awake()
    {
        itemName = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        itemType = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        value = transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        equipCharacter = transform.GetChild(4).GetChild(0).GetComponent<Image>();
        equipText = transform.GetChild(4).GetChild(1).GetComponent <TextMeshProUGUI>();
        equipButton = transform.parent.GetChild(5).gameObject;
    }
    public void UpdateItemPanel(int id, CharacterItemSprite characterFromSlot)
    {
        Item item = ItemDatabase.Instance.GetItem(id);
        itemName.text = item.itemName;
        switch (item.equipType)
        {
            case EqiupType.Claymore:
                itemType.text = "양손검";
                value.text = item.value.ToString();
                break;
            case EqiupType.Catalyst:
                itemType.text = "법구";
                value.text = item.value.ToString();
                break;
            case EqiupType.Bow:
                itemType.text = "활";
                value.text = item.value.ToString();
                break;
            case EqiupType.Pole:
                itemType.text = "장병기";
                value.text = item.value.ToString();
                break;
            case EqiupType.Sword:
                itemType.text = "한손검";
                value.text = item.value.ToString();
                break;
        }
        if(characterFromSlot == CharacterItemSprite.None)
        {
            equipCharacter.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            equipCharacter.transform.parent.gameObject.SetActive(true);
            equipCharacter.sprite = ItemDatabase.Instance.GetCharacterSprite(characterFromSlot);
            switch (characterFromSlot)
            {
                case CharacterItemSprite.Beidou:
                    equipText.text = "북두 장착중";
                    break;
                case CharacterItemSprite.Kokomi:
                    equipText.text = "코코미 장착중";
                    break;
                case CharacterItemSprite.Wriothesley:
                    equipText.text = "라이오슬리 장착중";
                    break;
                case CharacterItemSprite.Yoimiya:
                    equipText.text = "요이미야 장착중";
                    break;
            }
        }


        //if(EquipManager.Instance.character.Equals(characterFromSlot))
        //{
        //    equipButton.SetActive(false);
        //}
        //else
        //{
        //    equipButton.SetActive(true);
        //}
    }
}
