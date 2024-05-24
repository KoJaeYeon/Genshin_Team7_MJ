using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PartyManager : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    private Character[] activeCharacters;
    private int currentCharacterIndex = 0;

    public Transform spawnPosition;
    public GameObject playerParent;
    public GameObject particle;


    private void Awake()
    {
        InitializeCharacters();
    }

    private void Update()
    {
        HandleCharacterSwitchInput();
        UpdateAllCharacterCooldowns();
    }

    private void InitializeCharacters()
    {
        activeCharacters = new Character[characterPrefabs.Length];

        for (int i = 0; i < characterPrefabs.Length; i++)
        {
            GameObject characterObj = Instantiate(characterPrefabs[i], playerParent.transform);
            Character character = characterObj.GetComponent<Character>();
            activeCharacters[i] = character;
            activeCharacters[i].gameObject.SetActive(i == currentCharacterIndex);

            character.InitializeCharacterStats();
            character.InitializeCharacter();

            if (spawnPosition != null)
            {
                activeCharacters[0].transform.position = spawnPosition.position;
            }
        }
    }

    public void SwitchCharacter(int characterIndex)
    {
        if (PlayerController._isGliding) return;

        if (IsValidCharacterIndex(characterIndex))
        {
            Vector3 currentPosition = activeCharacters[currentCharacterIndex].transform.position;
            Quaternion currentRotation = activeCharacters[currentCharacterIndex].transform.rotation;

            activeCharacters[currentCharacterIndex].gameObject.SetActive(false);

            currentCharacterIndex = characterIndex;
            activeCharacters[currentCharacterIndex].gameObject.SetActive(true);

            activeCharacters[currentCharacterIndex].transform.position = currentPosition;
            activeCharacters[currentCharacterIndex].transform.rotation = currentRotation;

            UpdateCharacterController(activeCharacters[currentCharacterIndex].characterData);
            UpdateSkillUI();

            //캐릭터 변경할 때 이펙트 생성
            StartCoroutine(PlaySwtichEffect());
        }
        else
        {
            Debug.Log("Invalid character index");
        }
    }

    private void HandleCharacterSwitchInput()
    {
        if (Keyboard.current.digit1Key.isPressed && currentCharacterIndex != 0) SwitchCharacter(0);
        if (Keyboard.current.digit2Key.isPressed && currentCharacterIndex != 1) SwitchCharacter(1);
        if (Keyboard.current.digit3Key.isPressed && currentCharacterIndex != 2) SwitchCharacter(2);
        if (Keyboard.current.digit4Key.isPressed && currentCharacterIndex != 3) SwitchCharacter(3);
    }

    private void UpdateAllCharacterCooldowns()
    {
        foreach(var character in activeCharacters)
        {
            if(character != null)
            {
                character.UpdateSkillTimers();
            }
        }
    }

    private void UpdateSkillUI()
    {
        Character currentCharacter = activeCharacters[currentCharacterIndex];
        if(currentCharacter != null)
        {
            UIManager.Instance.SkiilCooldown(currentCharacter.GetSkillCooldownTimer());
            UIManager.Instance.BurstGage(currentCharacter.GetElementalEnergy());

            if(currentCharacter.IsSkillActive())
            {
                UIManager.Instance.SkiilCooldown(currentCharacter.GetSkillCooldownTimer());
            }
            else
            {
                UIManager.Instance.SkiilCooldown(0);
            }
        }
    }

    private bool IsValidCharacterIndex(int characterIndex)
    {
        return characterIndex >= 0 && characterIndex < activeCharacters.Length && !activeCharacters[characterIndex].isDead;
    }

    private void UpdateCharacterController(CharacterData data)
    {
        if (data == null) return;

        CharacterController controller = playerParent.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.center = data.controllerCenter;
            controller.radius = data.controllerRadius;
            controller.height = data.controllerHeight;
        }
    }

    IEnumerator PlaySwtichEffect()
    {
        if (particle == null)
        {
            Debug.Log("뿅 하는 파티클 없음");
            yield break;
        }
        particle.SetActive(false);
        particle.SetActive(true);
        yield return new WaitForSeconds(1);
        particle.SetActive(false);
    }

    public Character GetCurrentCharacter()
    {
        return activeCharacters[currentCharacterIndex].GetComponent<Character>();
    }
}
