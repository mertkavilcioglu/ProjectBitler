using UnityEngine;
using System.Linq;
using System.Collections;

public class FriendlyCanon : MonoBehaviour
{
    private Animator anim;
    public GameObject canonBallPrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    public float range = 10f;

    private float nextFireTime = 0f;
    private bool isFire = false;
    private bool firstTimeInRange = true; // Enemy ilk kez menzile girdi mi?

    private void Start()
    {
        anim = GetComponent<Animator>(); // Animator bileşenini al
    }

    private void Update()
    {
        GameObject[] enemyObjs = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject nearestTarget = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemyObjs)
        {
            float distance = Vector2.Distance(firePoint.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTarget = enemy;
            }
        }

        if (nearestTarget != null && nearestDistance <= range)
        {
            isFire = true;

            // Eğer ilk kez menzile girdiyse, 2 saniye bekleyip ateş etmeli
            if (firstTimeInRange)
            {
                firstTimeInRange = false; // İlk giriş tamamlandı
                StartCoroutine(DelayedFire(nearestTarget.transform.position));
            }
            else if (Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + fireRate;
                FireCanonBall(nearestTarget.transform.position);
            }
        }
        else
        {
            isFire = false;
            firstTimeInRange = true; // Enemy menzilden çıkınca sıfırla
        }

        if (anim != null)
        {
            anim.SetBool("IsFire", isFire); // **IsFire parametresi ile animasyon kontrolü**
        }
    }

    IEnumerator DelayedFire(Vector2 targetPosition)
    {
        yield return new WaitForSeconds(2f); // İlk girişte 2 saniye bekle
        FireCanonBall(targetPosition);
        nextFireTime = Time.time + fireRate; // Normal ateş etme döngüsüne gir
    }

    void FireCanonBall(Vector2 targetPosition)
    {   
        if (canonBallPrefab != null && firePoint != null)
        {
            GameObject ball = Instantiate(canonBallPrefab, firePoint.position, Quaternion.identity);

            Vector2 direction = (targetPosition - (Vector2)firePoint.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            ball.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}

