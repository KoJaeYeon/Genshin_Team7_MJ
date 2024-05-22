using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PropertyText_Relic : MonoBehaviour
{
    TextMeshProUGUI[] value;
    GameObject[] none_Images;
    Image[] relic_Images;

    private void Awake()
    {
        value = new TextMeshProUGUI[4];
        none_Images = new GameObject[5];
        relic_Images = new Image[5];
        for (int i = 0; i < value.Length; i++)
        {
            value[i] = transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>();
        }
        for (int i = 0; i < relic_Images.Length; i++)
        {
            none_Images[i] = transform.GetChild(6).GetChild(i).GetChild(0).gameObject;
            relic_Images[i] = transform.GetChild(6).GetChild(i).GetChild(1).GetComponent<Image>();
        }


    }

    private void OnEnable()
    {
        UpdatePanel();
    }

    public void UpdatePanel()
    {
        EquipStats equipStats = EquipManager.Instance.GetEquip(EquipManager.Instance.character);
        for(int i = 0; i < 5; i++)
        {
            if (equipStats.itemSlotKeys[i+1] != -1)
            {
                Item item = InventoryManager.Instance.GetRelicItem(equipStats.itemSlotKeys[i+1]);
                relic_Images[i].sprite = ItemDatabase.Instance.GetItemSprite(item.id);
                none_Images[i].SetActive(false);
                relic_Images[i].gameObject.SetActive(true);
            }
            else
            {
                relic_Images[i].gameObject.SetActive(false);
                none_Images[i].gameObject.SetActive(true);
            }
        }
        Debug.Log(equipStats.flowerHealth + " " + equipStats.sandTime_HelathPercent);
        value[0].text = ((int)(equipStats.flowerHealth * (100 + equipStats.sandTime_HelathPercent) / 100)).ToString();
        value[1].text = ((int)(equipStats.featherDamage * (100 + equipStats.trohphy_AttackPercent) / 100)).ToString();

    }
}
