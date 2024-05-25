using UnityEngine;
public class Arrow : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector3 _initialPosition;
    private bool _hasHitTarget = false;

    public float damage = 10f;
    public float maxDistance = 50f;
    public Character character;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _initialPosition = transform.position;
    }

    private void Update()
    {
        if (!_hasHitTarget && Vector3.Distance(_initialPosition, transform.position) > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                Element currentElement = character != null ? character.GetCurrentWeaponElement() : Element.Normal;
                enemy.TakeDamage(damage, currentElement, character);
            }
            _hasHitTarget = true;
            Destroy(gameObject);
        }
    }

    public void Shoot(Vector3 direction, float force)
    {
        _rigidbody.AddForce(direction * force, ForceMode.Impulse);
    }
}
