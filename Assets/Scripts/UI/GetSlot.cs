using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetSlot : MonoBehaviour
{
    Image image;
    TextMeshProUGUI itemName;
    TextMeshProUGUI itemCount;
    private void Awake()
    {
        image = transform.GetChild(0).GetComponent<Image>();
        itemName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        itemCount = transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    public void Init(Item item)
    {
        image.sprite = ItemDatabase.Instance.GetItemSprite(item.id);
        itemName.text = item.itemName;
        if (item.count > 1)
        {
            itemCount.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            itemCount.transform.parent.gameObject.SetActive(false);
        }
        itemCount.text = item.count.ToString();
    }

    public void Init(ChestType chestType)
    {
        switch (chestType)
        {
            case ChestType.Jungyo:
                image.sprite = ItemDatabase.Instance.GetChestSprite(0);
                itemName.text = "정교한 보물상자";
                break;
            case ChestType.Jingui:
                image.sprite = ItemDatabase.Instance.GetChestSprite(1);
                itemName.text = "진귀한 보물상자";
                break;
        }
        itemCount.transform.parent.gameObject.SetActive(false);
    }
}
