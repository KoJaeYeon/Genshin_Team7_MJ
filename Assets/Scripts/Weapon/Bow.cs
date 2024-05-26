using UnityEngine;

public class Bow : Weapon
{
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public float arrowSpeed = 1f;

    public override void UseWeapon()
    {
        Shoot();
        SoundManager.Instance.PlayEffect("Yoimiya_Attack");
        
    }

    private void Shoot()
    {
        if (arrowPrefab != null && arrowSpawnPoint != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = arrowSpawnPoint.forward * arrowSpeed;
            }
        }
    }
}
