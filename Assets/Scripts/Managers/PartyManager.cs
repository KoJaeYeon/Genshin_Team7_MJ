using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public List<CharacterInfo> party = new List<CharacterInfo>();
    public int maxPartySize = 4;
    private int activeCharacterIndex = 0;
    public CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Start()
    {
        
    }

    public void AddCharacterToParty(CharacterInfo newCharacter)
    {
        if(party.Count < maxPartySize)
        {
            party.Add(newCharacter);
        }
    }

    public void RemoveCharacterFromParty(CharacterInfo character)
    {
        if (party.Contains(character))
        {
            party.Remove(character);
        }
    }

    public void SwitchActiveCharacter(int newIndex)
    {
        if (newIndex >= 0 && newIndex < party.Count)
        {
            party[activeCharacterIndex].SetActive(false);
            activeCharacterIndex = newIndex;
            party[activeCharacterIndex].SetActive(true);

            Transform cameraLook = party[activeCharacterIndex].transform.Find("CameraLook");
            if(cameraLook != null)
            {
                cinemachineVirtualCamera.Follow = cameraLook;
            }
        }
    }

    public CharacterInfo GetActiveCharacter()
    {
        return party[activeCharacterIndex]; 
    }
}
