using Cinemachine;
using System.Collections;
using System.Collections.Generic;
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
        partyManager.AddCharacterToParty(instance_Beidou);

        CatalystCharacter instance_Wrio = Instantiate(prefab_Wrio);
        instance_Wrio.characterData = characterDatas[1];
        instance_Wrio.gameObject.SetActive(false);
        partyManager.AddCharacterToParty(instance_Beidou);

        CatalystCharacter instance_Kokomi = Instantiate(prefab_Kokomi);
        instance_Kokomi.characterData = characterDatas[2];
        instance_Kokomi.gameObject.SetActive(false);
        partyManager.AddCharacterToParty(instance_Beidou);

        BowCharacter instance_Yoimiya = Instantiate(prefab_Yoimiya);
        instance_Yoimiya.characterData = characterDatas[3];
        instance_Yoimiya.thirdPersonCamera = thirdPersonCamera;
        instance_Yoimiya.firstPersonCamera = firstPersonCamera;
        instance_Yoimiya .gameObject.SetActive(false);
        partyManager.AddCharacterToParty(instance_Beidou);

        partyManager.SwitchActiveCharacter(0);
    }

    private void Update()
    {
        HandleCharacterSwitch();
    }

    private void HandleCharacterSwitch()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            partyManager.SwitchActiveCharacter(0);
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            partyManager.SwitchActiveCharacter(1);
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            partyManager.SwitchActiveCharacter(2);
        }
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            partyManager.SwitchActiveCharacter(3);
        }
    }
}
