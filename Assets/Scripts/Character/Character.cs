using UnityEngine;

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

    public bool isDead { get; protected set; } = false;

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
        weapons[currentWeaponIndex].gameObject.SetActive(true);
    }

    private void Update()
    {
        HandleInput();
        UpdateSkillTimers();
    }

    private void HandleInput()
    {
        if (_input.attack)
        {
            Attack();
            _input.attack = false;
        }
        if (_input.skill && skillCooldownTimer <= 0)
        {
            UseElementalSkill();
            skillCooldownTimer = skillCooldown;
            isSkillActive = true;
            skillDurationTimer = skillDuration;
        }

        if (_input.burst)
        {
            UseElementalBurst();
        }
    }

    public void UpdateSkillTimers()
    {
        if (isSkillActive)
        {
            skillDurationTimer -= Time.deltaTime;

            if (skillDurationTimer <= 0f)
            {
                ResetSkill();
                isSkillActive = false;
            }
        }

        if (skillCooldownTimer > 0f)
        {
            skillCooldownTimer -= Time.deltaTime;
        }
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
        if (weaponIndex >= 0 && weaponIndex < weapons.Length)
        {
            weapons[currentWeaponIndex].gameObject.SetActive(false);
            currentWeaponIndex = weaponIndex;
            weapons[weaponIndex].gameObject.SetActive(true);
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
        UIManager.Instance.BurstGage(currentElementalEnergy);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UIManager.Instance.Health(currentHealth / maxHealth);

        if (hasAnimator)
        {
            _animator.SetTrigger("TakeDamage");
        }
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

    public float GetSkillCooldownTimer()
    {
        return skillCooldownTimer;
    }

    public float GetElementalEnergy()
    {
        return currentElementalEnergy;
    }

    public bool IsSkillActive()
    {
        return skillCooldownTimer > 0f;
    }

    public abstract void Attack();
    public abstract void UseElementalSkill();
    public abstract void UseElementalBurst();
    protected void PerformAttackAnimation()
    {
        if (hasAnimator)
        {
            _animator.SetTrigger("Attack");
            _animator.SetBool("Attacking", true);
        }
        else
        {
            _animator.SetBool("Attacking", false);
        }
    }
}