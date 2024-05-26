using UnityEngine;
public class Arrow : MonoBehaviour
{
    public float damage = 10f;
    public float maxDistance = 50f;
    public Character character;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        Debug.Log("Arrow fired from position: " + startPosition);
    }

    private void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Debug.Log("Arrow reached max distance and is being destroyed.");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Arrow triggered with: " + other.name);

        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                Element currentElement = character != null ? character.GetCurrentWeaponElement() : Element.Normal;
                Debug.Log("Enemy hit: " + enemy.name + ", Damage: " + damage + ", Element: " + currentElement);
                enemy.TakeDamage(damage, currentElement, character);
            }
            Debug.Log("Arrow hit an enemy and is being destroyed.");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Arrow hit a non-enemy object and is being destroyed.");
            Destroy(gameObject);
        }
    }
}
