using TMPro;
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

        // Initialize slider values based on SoundManager settings
        slider_Main.value = AudioListener.volume;
        slider_Volume.value = SoundManager.Instance.effectSource.volume;
        slider_Music.value = SoundManager.Instance.bgmSource.volume;
    }

    public void ValueMain()
    {
        float volume = slider_Main.value;
        text_Main.text = volume.ToString();
        images_Main[0].SetActive(volume > 0);
        images_Main[1].SetActive(volume > 6);
        AudioListener.volume = volume / 10;
    }

    public void ValueMusic()
    {
        float volume = slider_Music.value;
        text_Music.text = volume.ToString();
        images_Music[0].SetActive(volume > 0);
        images_Music[1].SetActive(volume > 6);
        SoundManager.Instance.bgmSource.volume = volume / 10;
    }

    public void ValueEffect()
    {
        float volume = slider_Volume.value;
        text_Volume.text = volume.ToString();
        images_Volume[0].SetActive(volume > 0);
        images_Volume[1].SetActive(volume > 6);
        SoundManager.Instance.effectSource.volume = volume / 10;
        SoundManager.Instance.effectSource2.volume = volume / 10;
    }

    public void ValueHorizontal()
    {
        text_Horizontal.text = slider_Horizontal.value.ToString();
        playerInputHandler.look_Horizontal = slider_Horizontal.value;
    }

    public void ValueVertical()
    {
        text_Vertical.text = slider_Vertical.value.ToString();
        playerInputHandler.look_Vertical = slider_Vertical.value;
    }
}