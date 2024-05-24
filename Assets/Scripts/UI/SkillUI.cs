using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public TextMeshProUGUI elementalSkill_Text_Cooldown;
    public TextMeshProUGUI elementalBurst_Text_Cooldown;

    public Image elementalBurst_Full_Image;

    public void Elemental_Cooldown(float value)
    {
        elementalSkill_Text_Cooldown.text = value.ToString();
        elementalBurst_Text_Cooldown.gameObject.SetActive(value <= 0);
    }

    public void Elemental_Burst_Cooldown(float value)
    {
        elementalBurst_Text_Cooldown.text = value.ToString();
        elementalBurst_Text_Cooldown.gameObject.SetActive(value <= 0);
    }

    public void ElementalBurst_Gage(float value)
    {
        elementalBurst_Full_Image.fillAmount = value;
    }
}
