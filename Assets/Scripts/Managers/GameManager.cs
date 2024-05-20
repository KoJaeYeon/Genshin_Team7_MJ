using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public PartyManager partyManager;
    public ClaymoreCharacter prefab_Beidou;
    public CatalystCharacter prefab_Wrio;
    public CatalystCharacter prefab_Kokomi;
    public BowCharacter prefab_Yoimiya;

    public CharacterData[] characterDatas;

    public CinemachineVirtualCamera thirdPersonCamera;
    public CinemachineVirtualCamera firstPersonCamera;

    private void Start()
    {
        ClaymoreCharacter instance_Beidou = Instantiate(prefab_Beidou);
        instance_Beidou.characterData = characterDatas[0];
        instance_Beidou.gameObject.SetActive(true);
        partyManager.AddCharacterToParty(instance_Beidou);

        CatalystCharacter instance_Wrio = Instantiate(prefab_Wrio);
        instance_Wrio.characterData = characterDatas[1];
        instance_Wrio.gameObject.SetActive(false);
        partyManager.AddCharacterToParty(instance_Wrio);

        CatalystCharacter instance_Kokomi = Instantiate(prefab_Kokomi);
        instance_Kokomi.characterData = characterDatas[2];
        instance_Kokomi.gameObject.SetActive(false);
        partyManager.AddCharacterToParty(instance_Kokomi);

        BowCharacter instance_Yoimiya = Instantiate(prefab_Yoimiya);
        instance_Yoimiya.characterData = characterDatas[3];
        instance_Yoimiya.thirdPersonCamera = thirdPersonCamera;
        instance_Yoimiya.firstPersonCamera = firstPersonCamera;
        instance_Yoimiya .gameObject.SetActive(false);
        partyManager.AddCharacterToParty(instance_Yoimiya);

       
    }

    private void Update()
    {
        HandleCharacterSwitch();
    }

    private void HandleCharacterSwitch()
    {
        if (Keyboard.current.numpad1Key.wasPressedThisFrame)
        {
            Debug.Log("Switch 1");
            partyManager.SwitchActiveCharacter(0);
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            Debug.Log("Switch 2");
            partyManager.SwitchActiveCharacter(1);
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            Debug.Log("Switch 3");
            partyManager.SwitchActiveCharacter(2);
        }
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            Debug.Log("Switch 4");
            partyManager.SwitchActiveCharacter(3);
        }
    }
}
