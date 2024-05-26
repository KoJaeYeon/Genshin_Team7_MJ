using UnityEngine;

public class Bow : Weapon
{
    public float range = 50f; 
    public float damage = 10f; 
    public Transform arrowSpawnPoint;

    public override void UseWeapon(Transform target = null)
    {
        Vector3 direction;

        if (target != null)
        {
            direction = (target.position - arrowSpawnPoint.position).normalized;
        }
        else
        {
            direction = arrowSpawnPoint.forward;
        }

        RaycastHit hit;
        if (Physics.Raycast(arrowSpawnPoint.position, direction, out hit, range))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    Element currentElement = character != null ? character.GetCurrentWeaponElement() : Element.Normal;
                    enemy.TakeDamage(damage, currentElement, character);
                }
            }
        }

        Debug.DrawRay(arrowSpawnPoint.position, direction * range, Color.red, 2f); // 디버그 레이
    }
}
