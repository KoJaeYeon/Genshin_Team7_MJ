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

    private void Start()
    {
        activeCharacters = new GameObject[characterPrefabs.Length];

        for(int i = 0; i < characterPrefabs.Length; i++)
        {
            activeCharacters[i] = Instantiate(characterPrefabs[i]);
            activeCharacters[i].SetActive(i == currentCharacterIndex);
        }
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
            activeCharacters[currentCharacterIndex].SetActive(false);

            currentCharacterIndex = characterIndex;

            activeCharacters[currentCharacterIndex].SetActive(true);
        }
        else
        {
            Debug.Log("Invalid character index");
        }
    }

    public CharacterInfo GetCurrentCharacter()
    {
        return activeCharacters[currentCharacterIndex].GetComponent<CharacterInfo>();
    }
}
