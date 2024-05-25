using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class SettingValue : MonoBehaviour
{
    public PlayerInputHandler playerInputHandler;

    public Slider slider_Main;
    TextMeshProUGUI text_Main;
    public GameObject[] images_Main;

    public Slider slider_Music;
    TextMeshProUGUI text_Music;
    public GameObject[] images_Music;

    public Slider slider_Volume;
    TextMeshProUGUI text_Volume;
    public GameObject[] images_Volume;


    public Slider slider_Horizontal;
    TextMeshProUGUI text_Horizontal;

    public Slider slider_Vertical;
    TextMeshProUGUI text_Vertical;


    private void Awake()
    {
        text_Main = slider_Main.transform.parent.GetComponentInChildren<TextMeshProUGUI>();
        text_Volume = slider_Volume.transform.parent.GetComponentInChildren<TextMeshProUGUI>();
        text_Music = slider_Music.transform.parent.GetComponentInChildren<TextMeshProUGUI>();
        text_Horizontal = slider_Horizontal.transform.parent.GetComponentInChildren<TextMeshProUGUI>();
        text_Vertical = slider_Vertical.transform.parent.GetComponentInChildren<TextMeshProUGUI>();
    }
    public void ValueMain()
    {
        text_Main.text = slider_Main.value.ToString();
        images_Main[0].SetActive(slider_Main.value > 0);
        images_Main[1].SetActive(slider_Main.value > 6);
    }

    public void ValueMusic()
    {
        text_Music.text = slider_Music.value.ToString();
        images_Music[0].SetActive(slider_Music.value > 0);
        images_Music[1].SetActive(slider_Music.value > 6);
    }
    public void ValueEffect()
    {
        text_Volume.text = slider_Volume.value.ToString();
        images_Volume[0].SetActive(slider_Volume.value > 0);
        images_Volume[1].SetActive(slider_Volume.value > 6);
    }

    public void ValueHorizontal()
    {
        text_Horizontal.text = slider_Horizontal.value.ToString();
        playerInputHandler.look_Horizontal = slider_Horizontal.value;
    }

    public void ValueVerical()
    {
        text_Vertical.text = slider_Vertical.value.ToString();
        playerInputHandler.look_Vertical = slider_Vertical.value;
    }
}
