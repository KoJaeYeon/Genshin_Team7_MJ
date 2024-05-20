using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PartyManager : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    private GameObject[] activeCharacters;
    private int currentCharacterIndex = 0;

    public CameraSetting FollowCamera;

    private Animator currentAnimator;
    private AnimatorStateInfo currentStateInfo;
    private float currentAnimatorTime;

    private void Start()
    {
        activeCharacters = new GameObject[characterPrefabs.Length];

        for(int i = 0; i < characterPrefabs.Length; i++)
        {
            activeCharacters[i] = Instantiate(characterPrefabs[i]);
            activeCharacters[i].SetActive(i == currentCharacterIndex);
        }

        FollowCamera.SetTarget(activeCharacters[currentCharacterIndex].transform);
        currentAnimator = activeCharacters[currentCharacterIndex].GetComponent<Animator>();
    }

    private void Update()
    {
        if (Keyboard.current.digit1Key.isPressed) SwitchCharacter(0);
        if (Keyboard.current.digit2Key.isPressed) SwitchCharacter(1);
        if (Keyboard.current.digit3Key.isPressed) SwitchCharacter(2);
        if (Keyboard.current.digit4Key.isPressed) SwitchCharacter(3);
    }

    public void SwitchCharacter(int characterIndex)
    {
        if (characterIndex >= 0 && characterIndex < activeCharacters.Length)
        {
            currentStateInfo = currentAnimator.GetCurrentAnimatorStateInfo(0);
            currentAnimatorTime = currentStateInfo.normalizedTime;

            Vector3 currentPosition = activeCharacters[currentCharacterIndex].transform.position;
            Quaternion currentRotation = activeCharacters[currentCharacterIndex].transform.rotation;

            Vector3 cameraPosition = FollowCamera.transform.position;
            Quaternion cameraRotation = FollowCamera.transform.rotation;

            activeCharacters[currentCharacterIndex].SetActive(false);

            currentCharacterIndex = characterIndex;
            activeCharacters[currentCharacterIndex].SetActive(true);

            activeCharacters[currentCharacterIndex].transform.position = currentPosition;
            activeCharacters[currentCharacterIndex].transform.rotation = currentRotation;

            FollowCamera.SetTarget(activeCharacters[currentCharacterIndex].transform);
            FollowCamera.SetCameraSettings(cameraPosition, cameraRotation);

            currentAnimator = activeCharacters[currentCharacterIndex].GetComponent<Animator>();
            currentAnimator.Play(currentStateInfo.fullPathHash, 0, currentAnimatorTime);
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
