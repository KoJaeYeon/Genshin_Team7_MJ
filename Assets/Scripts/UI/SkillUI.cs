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
}
