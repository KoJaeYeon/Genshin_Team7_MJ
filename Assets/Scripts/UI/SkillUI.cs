using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public TextMeshProUGUI elementalSkill_Text_Cooldown;
    public TextMeshProUGUI elementalBurst_Text_Cooldown;

    public Image elementalSkill_Cool_Image;
    public Image elementalBurst_Cool_Image;
    public Image elementalBurst_Full_Image;

    public Image attack_Image;
    public Image skill_Image;
    public Image burst_Image;
    public Image burst_Full_Image;

    public void Elemental_Cooldown(float value)
    {
        elementalSkill_Text_Cooldown.transform.parent.gameObject.SetActive(value >= 0);
        elementalSkill_Text_Cooldown.text = value.ToString("0.0");
        elementalSkill_Cool_Image.fillAmount = value / 10;
    }

    public void Elemental_Burst_Cooldown(float value)
    {
        elementalBurst_Text_Cooldown.text = value.ToString("0.0");
        elementalBurst_Text_Cooldown.gameObject.SetActive(value <= 0);
        elementalBurst_Cool_Image.fillAmount = value/10;
    }

    public void ElementalBurst_Gage(float value)
    {
        elementalBurst_Full_Image.fillAmount = value / 100;
    }

    public void SetSkillImage(Sprite attack_Image, Sprite skill_Image, Sprite burst_Image, Sprite burst_Full_Image)
    {
        if(this.attack_Image != null)
        {
            this.attack_Image.sprite = attack_Image;
        }        
        this.skill_Image.sprite = skill_Image;
        this.burst_Image.sprite = burst_Image;
        this.burst_Full_Image.sprite = burst_Full_Image;
    }
}
