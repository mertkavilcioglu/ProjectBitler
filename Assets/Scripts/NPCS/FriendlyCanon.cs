using UnityEngine;

public class FriendlyCanon : MonoBehaviour
{
    
    public GameObject canonBallPrefab;   
    public Transform firePoint;          
    public float fireRate = 2f;         
    public float range = 10f;            

    private float nextFireTime = 0f;

    private void Update()
    {
        
        GameObject[] enemyObjs = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject nearestTarget = null;
        float nearestDistance = Mathf.Infinity;

        // FirePoint konumuna göre en yakın enemy objeyi tespit et.
        foreach (GameObject enemy in enemyObjs)
        {
            float distance = Vector2.Distance(firePoint.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTarget = enemy;
            }
        }

        // Eğer en yakın hedef bulunduysa, menzile girmişse ve ateş süresi geldiyse ateş et.
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
            // FirePoint'ten friendly canon ball prefab’ını oluştur.
            GameObject ball = Instantiate(canonBallPrefab, firePoint.position, Quaternion.identity);

            
            Vector2 direction = (targetPosition - (Vector2)firePoint.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            ball.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
