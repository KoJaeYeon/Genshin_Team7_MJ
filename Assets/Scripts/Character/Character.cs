using System.Collections.Generic;
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
    }
}