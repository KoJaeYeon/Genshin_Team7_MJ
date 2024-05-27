using UnityEngine;

public class BowCharacter : Character
{
    private Enemy target;
    public float detectionRange = 25.0f;
    public float detectionAngle = 60.0f;
    protected override void Start()
    {
        characterType = CharacterType.Ranged;
        base.Start();
    }

    private void Update()
    {
        DetectEnemies();

        if (_input.attack)
        {
            Attack();
            _input.attack = false;
        }
    }

    public override void Attack()
    {
        if(weapons.Length > 0)
        {
            if (target != null)
            {
                Debug.Log("Attempting to attack. Target is: " + (target != null ? target.name : "null"));

                Vector3 direction = (target.transform.position - transform.parent.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, lookRotation, Time.deltaTime * 10f);

                Debug.DrawRay(transform.parent.position, direction * detectionRange, Color.green);
            }

            weapons[currentWeaponIndex].UseWeapon();

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

    public override void UseElementalSkill()
    {
        EnchantWeapon(Element.Fire);
    }

    public override void UseElementalBurst()
    {
        if (currentElementalEnergy >= elementalBurstCost)
        {
            currentElementalEnergy -= elementalBurstCost;
        }
    }

    private void EnchantWeapon(Element element)
    {
        if (weapons.Length > 0)
        {
            weapons[currentWeaponIndex].element = element;
        }
    }

    protected override void ResetSkill()
    {
        base.ResetSkill();
    }

    private void DetectEnemies()
    {
        Collider[] hits = Physics.OverlapSphere(transform.parent.position, detectionRange);
        Enemy closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach(Collider hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();    
            if(enemy != null)
            {
                Vector3 directionToTarget = (enemy.transform.position - transform.parent.position).normalized;
                float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
                if(angleToTarget < detectionAngle / 2)
                {
                    float distanceToEnemy = Vector3.Distance(transform.parent.position, enemy.transform.position);
                    if(distanceToEnemy < closestDistance)
                    {
                        closestDistance = distanceToEnemy;
                        closestEnemy = enemy;
                    }
                }
            }
        }
        target = closestEnemy;

        if (target != null) 
        {
            Debug.Log($"Target detected: {target.name}, Distance: {closestDistance}"); 
        }
        else 
        {
            Debug.Log("No target detected within range and angle."); 
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.parent.position, detectionRange);

        Vector3 forward = transform.forward * detectionRange;
        Vector3 leftBoundary = Quaternion.Euler(0, -detectionAngle / 2, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, detectionAngle / 2, 0) * forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, forward);
        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);
    }
}
