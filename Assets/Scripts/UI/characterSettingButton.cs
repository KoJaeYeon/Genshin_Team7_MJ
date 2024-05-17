using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class characterItemSettingButton : MonoBehaviour
{
    public GameObject TextPanel_Parent;
    GameObject[] TextPanel;
    public GameObject select_Parent;
    GameObject selectAnimateImage;
    GameObject[] select_Panel_Icon_Image;
    private void Awake()
    {
        TextPanel = new GameObject[3];
        select_Panel_Icon_Image = new GameObject[3];
        selectAnimateImage = select_Parent.transform.GetChild(3).gameObject;
        for(int i = 0; i < 3; i++)
        {
            TextPanel[i] = TextPanel_Parent.transform.GetChild(i).gameObject;
            select_Panel_Icon_Image[i] = select_Parent.transform.GetChild(i).GetChild(0).gameObject;
        }
    }

    private void Start()
    {
        SelectActive(0);
    }

    public void SelectActive(int index)
    {
        for (int i = 0; i < TextPanel.Length; i++)
        {
            if (i == index)
            {
                TextPanel[i].SetActive(true);
                select_Panel_Icon_Image[i].gameObject.SetActive(false);
                selectAnimateImage.transform.position = select_Panel_Icon_Image[i].transform.position;
            }
            else
            {
                TextPanel[i].SetActive(false);
                select_Panel_Icon_Image[i].gameObject.SetActive(true);
            }
        }
    }
}
