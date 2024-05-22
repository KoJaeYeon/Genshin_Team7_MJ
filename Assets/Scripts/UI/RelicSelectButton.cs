using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class RelicSelectButton : MonoBehaviour
{
    public Transform relic_Content;
    Transform selectImage;
    Transform barImage;
    Image[] relic_Panel_Icon_Image;
    private void Awake()
    {
        relic_Panel_Icon_Image = new Image[5];

        selectImage = transform.GetChild(0);
        barImage = transform.GetChild(6).GetChild(0);
        for (int i = 0; i < 5; i++)
        {
            relic_Panel_Icon_Image[i] = transform.GetChild(i+1).GetComponent<Image>();
        }
    }

    public void relicPanelActive(int index)
    {
        InventoryManager.Instance.UnLoad_Relic();
        for (int i = 0; i < 5; i++)
        {
            if (i == index)
            {
                relic_Panel_Icon_Image[i].color = new Color(0.3665005f, 0.3791829f, 0.5471698f);
                selectImage.transform.position = relic_Panel_Icon_Image[i].transform.position;
                Vector3 vector3 = barImage.transform.position;
                vector3.x = relic_Panel_Icon_Image[i].transform.position.x;
                barImage.transform.position = vector3;

                InventoryManager.Instance.Load_Relic(relic_Content, (EqiupType)i+5);
                if (relic_Content.transform.childCount > 0)
                {
                    relic_Content.transform.GetChild(0).GetComponent<ItemSlot>().ShowData();
                }
            }
            else
            {
                relic_Panel_Icon_Image[i].color = Color.white;
            }
        }
    }
}
