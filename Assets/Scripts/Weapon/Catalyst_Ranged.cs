using UnityEngine;

public class Catalyst_Ranged : Weapon
{
    public float range = 20f;
    public float damage = 10f;
    public float detectionAngle = 45f;
    public LayerMask enemyLayer;

    public override void UseWeapon()
    {
        Enemy target = DetectEnemyInRange();
        if(target != null)
        {
            Vector3 directionToTarget = (target.transform.position - transform.parent.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0f, directionToTarget.z));
            transform.parent.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, range))
            {
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    Element currentElement = character != null ? character.GetCurrentWeaponElement() : Element.Normal;
                    enemy.TakeDamage(damage, currentElement, character);

                    if (character != null && currentElement != Element.Normal)
                    {
                        character.GainEnergy(character.energyGainPerHit);
                    }
                }
            }
        }
    }

    private Enemy DetectEnemyInRange()
    {
        Collider[] hits = Physics.OverlapSphere(transform.parent.position, range, enemyLayer);
        foreach(Collider hit in hits)
        {
            Vector3 directionToTarget = (hit.transform.position - transform.parent.position).normalized;
            float angleToTarget = Vector3.Angle(transform.parent.forward, directionToTarget);

            if(angleToTarget < detectionAngle / 2)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if(enemy != null)
                {
                    return enemy;
                }
            }
        }

        return null;
    }
}
