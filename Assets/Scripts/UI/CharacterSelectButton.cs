using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour
{
    public GameObject characterPanel_Panret;
    GameObject[] characterPanel;
    private void Awake()
    {
        characterPanel = new GameObject[3];
        for(int i = 0; i < 3; i++)
        {
            characterPanel[i] = characterPanel_Panret.transform.GetChild(i+1).gameObject;
        }
    }

    public void InventoryActive(int index)
    {
        for (int i = 0; i < characterPanel.Length; i++)
        {
            if (i == index)
            {
                characterPanel[i].SetActive(true);
            }
            else
            {
                characterPanel[i].SetActive(false);
            }
        }
    }
}
