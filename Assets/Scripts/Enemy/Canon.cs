using UnityEngine;

public class Canon : MonoBehaviour
{
    [Header("Canon Ayarlari")]
    
    public GameObject canonBallPrefab;
    
    public Transform firePoint;
    
    public float fireRate = 2f;
    
    public float range = 5f;

    private float nextFireTime = 0f;
    private Transform player;

    private void Start()
    {
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Player bulunamadı!");
        }
    }

    private void Update()
    {
        if (player == null) return;

        // FirePoint'ten oyuncuya olan mesafeyi kontrol et
        float distance = Vector2.Distance(firePoint.position, player.position);

        if (distance <= range && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            FireCanonBall(player.position);
        }
    }

    void FireCanonBall(Vector2 targetPosition)
    {
        if (canonBallPrefab != null && firePoint != null)
        {
            // FirePoint'ten canonball prefab’ını instantiate et.
            GameObject ball = Instantiate(canonBallPrefab, firePoint.position, Quaternion.identity);

            // FirePoint ile hedef arasındaki yönü hesapla.
            Vector2 direction = (targetPosition - (Vector2)firePoint.position).normalized;

            // Sadece canonball'ın rotasyonunu ayarla, hız CanonBall scripti tarafından ayarlanacak.
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            ball.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
