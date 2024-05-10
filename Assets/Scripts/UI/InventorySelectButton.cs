using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySelectButton : MonoBehaviour
{
    public GameObject inventroyPanelParent;
    GameObject[] InventoryPanel;
    private void Awake()
    {
        InventoryPanel = new GameObject[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            InventoryPanel[i] = transform.GetChild(i).gameObject;
        }
    }

    public void InventoryActive(int index)
    {
        for (int i = 0; i < InventoryPanel.Length; i++)
        {
            if (i == index) { InventoryPanel[i].SetActive(true); }
            else { InventoryPanel[i].SetActive(false); }
        }
    }
}
