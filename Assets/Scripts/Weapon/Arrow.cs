using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Arrow : Bow
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
            if(enemy != null)
            {
                Element currentElement = character != null ? character.GetCurrentWeaponElement() : Element.Normal;
                enemy.TakeDamage(damage, currentElement, character);
            }
            Destroy(gameObject);
        }
    }
}
