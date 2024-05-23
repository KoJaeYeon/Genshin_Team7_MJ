using TMPro;
using UnityEngine;

public class PropertyText_Weapon: MonoBehaviour
{
    TextMeshProUGUI itemName;
    TextMeshProUGUI itemType;
    TextMeshProUGUI value;

    private void Awake()
    {
        itemName = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        itemType = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        value = transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        UpdatePanel();
    }

    public void UpdatePanel()
    {
        EquipStats equipStats = EquipManager.Instance.GetEquip(EquipManager.Instance.character);
        Item item = InventoryManager.Instance.GetWeaponItem(equipStats.itemSlotKeys[0]);
        Debug.Log(item.itemName);
        itemName.text = item.itemName;
        switch (item.equipType)
        {
            case EqiupType.Claymore:
                itemType.text = "양손검";
                break;
            case EqiupType.Catalyst:
                itemType.text = "법구";
                break;
            case EqiupType.Bow:
                itemType.text = "활";
                break;
            case EqiupType.Pole:
                itemType.text = "장병기";
                break;
            case EqiupType.Sword:
                itemType.text = "한손검";
                break;
        }
        value.text = item.value.ToString();
    }
}
