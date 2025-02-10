using UnityEngine;
using System.Linq;  // Linq, dizileri birleştirmek için kullanılıyor.

public class Canon : MonoBehaviour
{
    
    public GameObject canonBallPrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    public float range = 5f;

    private float nextFireTime = 0f;

    private void Update()
    {
        // Sahnedeki hem "Player" hem de "Yeniceri" tagine sahip objeleri bul.
        GameObject[] playerObjs = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] friendSoldierObjs = GameObject.FindGameObjectsWithTag("FriendSoldier");
        
        // İki diziyi birleştiriyoruz.
        GameObject[] targets = playerObjs.Concat(friendSoldierObjs).ToArray();

        GameObject nearestTarget = null;
        float nearestDistance = Mathf.Infinity;

        // FirePoint pozisyonuna en yakın hedefi bul.
        foreach (GameObject target in targets)
        {
            float distance = Vector2.Distance(firePoint.position, target.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTarget = target;
            }
        }

        // Eğer bir hedef bulunduysa, hedef menziline giriyorsa ve ateş etme süresi geldiyse ateşle.
        if (nearestTarget != null && nearestDistance <= range && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            FireCanonBall(nearestTarget.transform.position);
        }
    }

    void FireCanonBall(Vector2 targetPosition)
    {
        if (canonBallPrefab != null && firePoint != null)
        {
            // FirePoint'ten canonball prefab’ını instantiate et.
            GameObject ball = Instantiate(canonBallPrefab, firePoint.position, Quaternion.identity);

            // Hedef ile FirePoint arasındaki yönü hesapla.
            Vector2 direction = (targetPosition - (Vector2)firePoint.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            ball.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
