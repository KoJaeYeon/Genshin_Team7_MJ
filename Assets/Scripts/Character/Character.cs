using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public abstract class Character : MonoBehaviour
{
    protected PlayerInputHandler _input;
    protected Animator _animator;

    public Weapon[] weapons;
    public CharacterType characterType { get; set; }

    public CharacterData characterData;

    protected int currentWeaponIndex = 0;

    protected bool isActive = false;
    protected bool hasAnimator;

    public float detectionRange = 10.0f;
    public float detectionAngle = 45.0f;

    protected float skillCooldown = 10.0f;
    protected float skillDuration = 5.0f;
    protected float skillCooldownTimer = 0f;
    protected float skillDurationTimer = 0f;
    protected bool isSkillActive = false;

    public float maxElementalEnergy = 100.0f;
    public float currentElementalEnergy = 0.0f;
    public float energyGainPerHit = 10.0f;
    public float energyGainOnKill = 20.0f;
    public float elementalBurstCost = 100.0f;

    public int level;
    public float maxHealth;
    public float currentHealth;
    public float attackPower;
    public float defensePower;

    protected virtual void Start()
    {
        if (characterData != null)
        {
            InitializeCharacterStats();
        }

        if (weapons != null &&  weapons.Length > 0)
        {
            InitializeWeapons();
        }

        _input = transform.parent.GetComponent<PlayerInputHandler>();
        _animator = GetComponent<Animator>();
        hasAnimator = TryGetComponent(out  _animator);
    }

    public void InitializeCharacterStats()
    {
        level = characterData.level;  
        maxHealth = characterData.baseHp;
        currentHealth = maxHealth;
        attackPower = characterData.baseAtk;
        defensePower = characterData.baseDef;
    }

    private void InitializeWeapons()
    {
        foreach(var weapon in weapons)
        {
            weapon.gameObject.SetActive(false);
            weapon.character = this;
            weapon.element = Element.Normal;
            Collider weaponCollider = weapon.GetComponent<Collider>();
            if(weaponCollider != null)
            {
                weaponCollider.isTrigger = true;
            }
        }
    }


    public void InitializeCharacter()
    {
        if(characterData != null)
        {
            CharacterController characterController = transform.parent.GetComponent<CharacterController>();
            if(characterController != null)
            {
                characterController.center = characterData.controllerCenter;
                characterController.radius = characterData.controllerRadius;
                characterController.height = characterData.controllerHeight;
            }
        }
    }


    public void SwitchWeapon(int weaponIndex)
    {
        if(weaponIndex >= 0 && weaponIndex < weapons.Length)
        {
            currentWeaponIndex = weaponIndex;
        }
    }

    public Element GetCurrentWeaponElement()
    {
        if(weapons.Length > 0)
        {
            return weapons[currentWeaponIndex].element;
        }
        return Element.Normal;
    }

    public void GainEnergy(float amount)
    {
        currentElementalEnergy = Mathf.Clamp(currentElementalEnergy + amount, 0, maxElementalEnergy);
        //UIManager.Instance.메서드이름(어떤 캐릭터, currentElementalEnergy);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        if (hasAnimator)
        {
            _animator.SetTrigger("TakeDamage");
        }
    }

    protected virtual void Die()
    {
        if (hasAnimator)
        {
            _animator.SetTrigger("Die");
        }
    }

    protected List<GameObject> DetectedEnemiesInRange()
    {
        List<GameObject> detectedEnemies = new List<GameObject>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);

        foreach(Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Vector3 directionToTarget = (collider.transform.position = transform.position).normalized;
                float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
                
                if(angleToTarget < detectionAngle/ 2)
                {
                    detectedEnemies.Add(collider.gameObject);
                }
            }
        }

        return detectedEnemies;
    }

    protected void FaceTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0 , direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
    }

    
    protected virtual void ResetSkill()
    {
        if(weapons.Length > 0)
        {
            weapons[currentWeaponIndex].element = Element.Normal;
        }
    }

    public void OnEnemyKilled()
    {
        GainEnergy(energyGainOnKill);
    }

    public abstract void Attack();
    public abstract void UseElementalSkill();
    public abstract void UseElementalBurst();

}