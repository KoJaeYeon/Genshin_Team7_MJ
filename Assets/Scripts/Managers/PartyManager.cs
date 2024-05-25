using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PartyManager : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    private Character[] activeCharacters;
    private int currentCharacterIndex = 0;

    public Transform spawnPosition;
    public GameObject playerParent;

    private Animator currentAnimator;
    private AnimatorStateInfo currentStateInfo;
    private float currentAnimatorTime;
    public GameObject particle;


    private void Awake()
    {
        activeCharacters = new Character[characterPrefabs.Length];

        for (int i = 0; i < characterPrefabs.Length; i++)
        {
            GameObject characterObj = Instantiate(characterPrefabs[i], playerParent.transform);
            Character character  = characterObj.GetComponent<Character>();
            activeCharacters[i] = character;
            EquipManager.Instance.playerCharacter[i] = character;
            activeCharacters[i].gameObject.SetActive(i == currentCharacterIndex);

            character.InitializeCharacterStats();
            character.InitializeCharacter();

            if (spawnPosition != null)
            {
                activeCharacters[0].transform.position = spawnPosition.position;
            }
        }
        currentAnimator = activeCharacters[currentCharacterIndex].GetComponent<Animator>();
    }

    private void Update()
    {
        if (Keyboard.current.digit1Key.isPressed && currentCharacterIndex != 0) SwitchCharacter(0);
        if (Keyboard.current.digit2Key.isPressed && currentCharacterIndex != 1) SwitchCharacter(1);
        if (Keyboard.current.digit3Key.isPressed && currentCharacterIndex != 2) SwitchCharacter(2);
        if (Keyboard.current.digit4Key.isPressed && currentCharacterIndex != 3) SwitchCharacter(3);
    }

    public void SwitchCharacter(int characterIndex)
    {
        if (PlayerController._isGliding) return;
        if (characterIndex >= 0 && characterIndex < activeCharacters.Length)
        {
            currentStateInfo = currentAnimator.GetCurrentAnimatorStateInfo(0);
            currentAnimatorTime = currentStateInfo.normalizedTime;

            Vector3 currentPosition = activeCharacters[currentCharacterIndex].transform.position;
            Quaternion currentRotation = activeCharacters[currentCharacterIndex].transform.rotation;

            activeCharacters[currentCharacterIndex].gameObject.SetActive(false);

            currentCharacterIndex = characterIndex;
            activeCharacters[currentCharacterIndex].gameObject.SetActive(true);

            activeCharacters[currentCharacterIndex].transform.position = currentPosition;
            activeCharacters[currentCharacterIndex].transform.rotation = currentRotation;

            currentAnimator = activeCharacters[currentCharacterIndex].GetComponent<Animator>();
            currentAnimator.Play(currentStateInfo.fullPathHash, 0, currentAnimatorTime);

            CharacterData data = activeCharacters[currentCharacterIndex].characterData;
            CharacterController controller = playerParent.GetComponent<CharacterController>();

            if (data != null && controller != null)
            {
                controller.center = data.controllerCenter;
                controller.radius = data.controllerRadius;
                controller.height = data.controllerHeight;
            }

            //캐릭터 변경할 때 이펙트 생성
            if (particle != null)
            {
                particle.SetActive(false);
                particle.SetActive(true);
            }
        }
        else
        {
            Debug.Log("Invalid character index");
        }
    }

    public Character GetCurrentCharacter()
    {
        return activeCharacters[currentCharacterIndex].GetComponent<Character>();
    }
}
