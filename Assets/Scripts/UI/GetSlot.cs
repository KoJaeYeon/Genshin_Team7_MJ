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

    public void Init(InteractableType chestType)
    {
        switch (chestType)
        {
            case InteractableType.Jungyo:
                image.sprite = ItemDatabase.Instance.GetChestSprite(0);
                itemName.text = "������ ��������";
                break;
            case InteractableType.Jingui:
                image.sprite = ItemDatabase.Instance.GetChestSprite(1);
                itemName.text = "������ ��������";
                break;
            case InteractableType.Mission:
                image.sprite = ItemDatabase.Instance.GetChestSprite(2);
                itemName.text = "���� ����";
                break;
            case InteractableType.Boss:
                image.sprite = ItemDatabase.Instance.GetChestSprite(3);
                itemName.text = "������ ���� ��ȯ";
                break;
        }
        itemCount.transform.parent.gameObject.SetActive(false);
    }

    public void SearchIndexAndSend() //������ ��ġ Ʈ���ſ� ������ ȹ���϶�� ����ϴ� �Լ�
    {
        for(int i = 0; i < transform.parent.childCount; i++)
        {
            if(transform == transform.parent.GetChild(i))
            {
                ItemSerachTrigger.Instance.GetItemIndex(i);
            }
        }

        
    }
}
