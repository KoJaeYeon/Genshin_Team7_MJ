using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetSlot : MonoBehaviour
{
    Image image;
    TextMeshProUGUI itemName;
    private void Awake()
    {
        image = transform.GetChild(0). GetComponent<Image>();
    }
    public void Init()
    {

    }
}
