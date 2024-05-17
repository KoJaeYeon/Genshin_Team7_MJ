using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ItemGetPanelSlot : MonoBehaviour
{
    Image image_J;
    TextMeshProUGUI itemName_J;
    
    
    private void Awake()
    {
       image_J = transform.GetChild(1).GetComponent<Image>();
       itemName_J = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    
    public void Init_J(Item item)
    {
        image_J.sprite = ItemDatabase.Instance.GetItemSprite(item.id);
        itemName_J.text = $"{item.itemName} x {item.count}";
       
    }
}
