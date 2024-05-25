using UnityEngine;

public class Bow : Weapon
{
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public float speed = 20f;

    public override void UseWeapon()
    {
        PerformShot();
    }

    public void PerformShot(Transform target = null)
    {
        GameObject arrowObject = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        Arrow arrow = arrowObject.GetComponent<Arrow>();

        if (arrow != null)
        {
            arrow.character = character; // 캐릭터 참조 설정
            Vector3 direction;
            if (target != null)
            {
                direction = (target.position - arrowSpawnPoint.position).normalized;
            }
            else
            {
                direction = arrowSpawnPoint.forward;
            }
            arrow.Shoot(direction, speed);
        }
    }
}
