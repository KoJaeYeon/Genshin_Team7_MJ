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
                characterName.text = "북두";
                break;
            case CharacterItemSprite.Kokomi:
                characterName.text = "코코미";
                break;
            case CharacterItemSprite.Wriothesley:
                characterName.text = "라이오슬리";
                break;
            case CharacterItemSprite.Yoimiya:
                characterName.text = "요이미야";
                break;
        }
    }
}
