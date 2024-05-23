using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jelifish : Catalyst_Ranged
{
    public float speed = 20f;
    public float damage = 10f;
    public Rigidbody rb;

    private void Start()
    {
        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, character != null ? character.GetCurrentWeaponElement() : Element.Normal);
            }
            Destroy(gameObject);
        }
    }
}
