using TMPro;
using UnityEngine;

public class PropertyText_Property: MonoBehaviour
{
    [SerializeField] private CharacterData[] characterData;

    TextMeshProUGUI characterName;
    TextMeshProUGUI hpCount;
    TextMeshProUGUI powerCount;
    TextMeshProUGUI defenceCount;

    private void Awake()
    {
        characterName = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        hpCount = transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        powerCount = transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>();
        defenceCount = transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        UpdatePanel();
    }

    public void UpdatePanel()
    {
        switch (EquipManager.Instance.character)
        {
            case CharacterItemSprite.Beidou:
                EquipStats Beidou_EquipStats = EquipManager.Instance.GetEquip(CharacterItemSprite.Beidou);
                characterName.text = "북두";
                hpCount.text = ((int)(characterData[0].baseHp * (1 + (Beidou_EquipStats.sandTime_HelathPercent) / 100f))).ToString();
                powerCount.text = ((int)((characterData[0].baseAtk + Beidou_EquipStats.weaponDamage + Beidou_EquipStats.featherDamage) * (1 + (Beidou_EquipStats.trohphy_AttackPercent) / 100f))).ToString();
                defenceCount.text = ((int)(characterData[0].baseDef * (1 + (Beidou_EquipStats.crown_defencePercent) / 100f))).ToString();
                break;

            case CharacterItemSprite.Kokomi:
                EquipStats Kokomi_EquipStats = EquipManager.Instance.GetEquip(CharacterItemSprite.Kokomi);
                characterName.text = "코코미";
                hpCount.text = ((int)(characterData[1].baseHp * (1 + (Kokomi_EquipStats.sandTime_HelathPercent) / 100f))).ToString();
                powerCount.text = ((int)((characterData[1].baseAtk + Kokomi_EquipStats.weaponDamage + Kokomi_EquipStats.featherDamage) * (1 + (Kokomi_EquipStats.trohphy_AttackPercent) / 100f))).ToString();
                defenceCount.text = ((int)(characterData[1].baseDef * (1 + Kokomi_EquipStats.crown_defencePercent) / 100f)).ToString();
                break;

            case CharacterItemSprite.Wriothesley:
                EquipStats Wrio_EquipStats = EquipManager.Instance.GetEquip(CharacterItemSprite.Wriothesley);
                characterName.text = "라이오슬리";
                hpCount.text = ((int)(characterData[2].baseHp * (1 + (Wrio_EquipStats.sandTime_HelathPercent) / 100f))).ToString();
                powerCount.text = ((int)((characterData[2].baseAtk + Wrio_EquipStats.weaponDamage + Wrio_EquipStats.featherDamage) * (1 + (Wrio_EquipStats.trohphy_AttackPercent) / 100f))).ToString();
                defenceCount.text = ((int)(characterData[2].baseDef * (1 + (Wrio_EquipStats.crown_defencePercent) / 100f))).ToString();
                break;

            case CharacterItemSprite.Yoimiya:
                EquipStats Yoimiya_EquipStats = EquipManager.Instance.GetEquip(CharacterItemSprite.Yoimiya);
                characterName.text = "요이미야";
                hpCount.text = ((int)(characterData[3].baseHp * (1 + (Yoimiya_EquipStats.sandTime_HelathPercent) / 100f))).ToString();
                powerCount.text = ((int)((characterData[3].baseAtk + Yoimiya_EquipStats.weaponDamage + Yoimiya_EquipStats.featherDamage) * (1 + (Yoimiya_EquipStats.trohphy_AttackPercent) / 100f))).ToString();
                defenceCount.text = ((int)(characterData[3].baseDef * (1 + (Yoimiya_EquipStats.crown_defencePercent) / 100f))).ToString();
                break;
        }
    }
}
