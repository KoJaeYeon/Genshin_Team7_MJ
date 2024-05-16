using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySelectButton : MonoBehaviour
{
    public GameObject inventoryPanel_Panret;
    GameObject[] inventoryPanel;
    GameObject selectImage;
    Image[] inventory_Panel_Icon_Image;
    private void Awake()
    {
        inventoryPanel = new GameObject[3];
        inventory_Panel_Icon_Image = new Image[3];
        selectImage = transform.GetChild(0).gameObject;
        for(int i = 0; i < 3; i++)
        {
            inventoryPanel[i] = inventoryPanel_Panret.transform.GetChild(i).gameObject;
            inventory_Panel_Icon_Image[i] = transform.GetChild(i+1).GetComponent<Image>();
        }
    }

    private void Start()
    {
        InventoryActive(0);
    }

    public void InventoryActive(int index)
    {
        for (int i = 0; i < inventoryPanel.Length; i++)
        {
            if (i == index)
            {
                inventoryPanel[i].SetActive(true);
                inventory_Panel_Icon_Image[i].color = new Color(0.3665005f, 0.3791829f, 0.5471698f);
                selectImage.transform.position = inventory_Panel_Icon_Image[i].transform.position;
            }
            else
            {
                inventoryPanel[i].SetActive(false);
                inventory_Panel_Icon_Image[i].color = Color.white;
            }
        }
    }
}
