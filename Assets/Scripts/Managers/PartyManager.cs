using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PartyManager : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public CharacterData[] characterDatas;
    private GameObject[] activeCharacters;
    private int currentCharacterIndex = 0;

    public Transform spawnPosition;
    public GameObject playerParent;

    private Animator currentAnimator;
    private AnimatorStateInfo currentStateInfo;
    private float currentAnimatorTime;

    private void Awake()
    {
        activeCharacters = new GameObject[characterPrefabs.Length];

        for (int i = 0; i < characterPrefabs.Length; i++)
        {
            GameObject character = Instantiate(characterPrefabs[i], playerParent.transform);
            activeCharacters[i] = character;
            activeCharacters[i].SetActive(i == currentCharacterIndex);

            CharacterData data = characterDatas[i];
            CharacterController controller = playerParent.GetComponent<CharacterController>();

            if (data != null && controller != null)
            {
                controller.center = data.controllerCenter;
                controller.radius = data.controllerRadius;
                controller.height = data.controllerHeight;
            }
        }

        if (spawnPosition != null)
        {
            activeCharacters[0].transform.position = spawnPosition.position;
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

            activeCharacters[currentCharacterIndex].SetActive(false);

            currentCharacterIndex = characterIndex;
            activeCharacters[currentCharacterIndex].SetActive(true);

            activeCharacters[currentCharacterIndex].transform.position = currentPosition;
            activeCharacters[currentCharacterIndex].transform.rotation = currentRotation;

            currentAnimator = activeCharacters[currentCharacterIndex].GetComponent<Animator>();
            currentAnimator.Play(currentStateInfo.fullPathHash, 0, currentAnimatorTime);

            CharacterData data = characterDatas[characterIndex];
            CharacterController controller = playerParent.GetComponent<CharacterController>();

            if(data != null && controller != null)
            {
                controller.center = data.controllerCenter;
                controller.radius = data.controllerRadius;
                controller.height = data.controllerHeight;
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
