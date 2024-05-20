using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CharacterInfo : MonoBehaviour
{
    public CharacterData characterData;
    public Animator animator;

    protected bool isActive = false;

    protected virtual void Start()
    {
        if(characterData != null)
        {
            InitializeCharacter();
        }
    }

    private void InitializeCharacter()
    {
        if (animator != null && characterData.controller != null)
        {
            animator.runtimeAnimatorController = characterData.controller;
        }
    }

    public virtual void SetActive(bool active)
    {
        isActive = active;
        gameObject.SetActive(active);

        
    }

    public abstract void Attack();

    protected virtual void Update()
    {
        
    }
}