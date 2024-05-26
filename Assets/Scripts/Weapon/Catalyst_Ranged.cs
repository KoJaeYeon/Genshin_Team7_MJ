using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catalyst_Ranged : Weapon
{
    public float range = 50f; 
    public float damage = 10f; 
    public Transform projectileSpawnPoint;

    public override void UseWeapon(Transform target = null)
    {
        Vector3 direction;

        if (target != null)
        {
            direction = (target.position - projectileSpawnPoint.position).normalized;
        }
        else
        {
            direction = projectileSpawnPoint.forward;
        }

        RaycastHit hit;
        if (Physics.Raycast(projectileSpawnPoint.position, direction, out hit, range))
        {
            Debug.Log($"Hit detected: {hit.collider.name} at distance: {hit.distance}");

            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Hit an enemy!");

                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    Debug.Log("Enemy component found, applying damage.");
                    Element currentElement = character != null ? character.GetCurrentWeaponElement() : Element.Normal;
                    enemy.TakeDamage(damage, currentElement, character);
                }
                else
                {
                    Debug.LogWarning("Hit object with Enemy tag but no Enemy component found.");
                }
            }
            else
            {
                Debug.Log("Hit object is not an enemy.");
            }
        }
        else
        {
            Debug.Log("No hit detected.");
        }

        Debug.DrawRay(projectileSpawnPoint.position, direction * range, Color.red, 2f);
    }
}
