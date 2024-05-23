using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Character : MonoBehaviour
{
    public Weapon[] weapons;
    public CharacterType characterType { get; set; }

    protected int currentWeaponIndex = 0;

    protected bool isActive = false;

    public float detectionRange = 10.0f;
    public float detectionAngle = 45.0f;

    public float skillCooldown = 10.0f;
    public float skillDuration = 5.0f;
    private float skillCooldownTimer = 0f;
    private float skillDurationTimer = 0f;
    private bool isSkillActive = false;

    public float maxElementalEnergy = 100.0f;
    public float currentElementalEnergy = 0.0f;
    public float energyGainPerHit = 10.0f;
    public float energyGainOnKill = 20.0f;
    public float elementalBurstCost = 100.0f;

    protected virtual void Start()
    {
        if(weapons != null &&  weapons.Length > 0)
        {
            InitializeWeapons();
        }
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

    public virtual void Attack()
    {
        if(weapons.Length > 0)
        {
            weapons[currentWeaponIndex].UseWeapon();
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

    public void UseElementalBurst()
    {
        if(currentElementalEnergy >= elementalBurstCost)
        {
            currentElementalEnergy -= elementalBurstCost;
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

    protected virtual void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Attack();
        }
        if (Keyboard.current.eKey.wasPressedThisFrame && skillCooldownTimer <= 0)
        {
            UseElementalSkill();
            skillCooldownTimer = skillCooldown;
            isSkillActive = true;
            skillDurationTimer = skillDuration;
        }

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            UseElementalBurst();
        }

        if (isSkillActive)
        {
            skillDurationTimer -= Time.deltaTime;
            Debug.Log(skillDurationTimer.ToString());
            if(skillDurationTimer <= 0f)
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

    public abstract void UseElementalSkill();

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
}