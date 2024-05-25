using TMPro;
using UnityEngine;

public class PropertyText_Property: MonoBehaviour
{
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
                characterName.text = "�ϵ�";
                break;
            case CharacterItemSprite.Kokomi:
                characterName.text = "���ڹ�";
                break;
            case CharacterItemSprite.Wriothesley:
                characterName.text = "���̿�����";
                break;
            case CharacterItemSprite.Yoimiya:
                characterName.text = "���̹̾�";
                break;
        }
    }
}
