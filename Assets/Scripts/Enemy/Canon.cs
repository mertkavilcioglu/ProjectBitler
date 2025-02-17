using UnityEngine;
using System.Linq;
using System.Collections;

public class Canon : MonoBehaviour
{
    private Animator anim;
    public GameObject canonBallPrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    public float range = 5f;

    private float nextFireTime = 0f;
    private bool isFiring = false; 
    private bool firstTimeInRange = true; // Oyuncu ilk kez menzile girdi mi?

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        GameObject[] playerObjs = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] friendSoldierObjs = GameObject.FindGameObjectsWithTag("FriendSoldier");

        GameObject[] targets = playerObjs.Concat(friendSoldierObjs).ToArray();

        GameObject nearestTarget = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            float distance = Vector2.Distance(firePoint.position, target.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTarget = target;
            }
        }

        if (nearestTarget != null && nearestDistance <= range)
        {
            isFiring = true;

            // Eğer ilk kez menzile girdiyse, bekleme başlat
            if (firstTimeInRange)
            {
                firstTimeInRange = false; // Artık ilk giriş değil
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
            isFiring = false;
            firstTimeInRange = true; // Oyuncu menzilden çıktığında tekrar aktif et
        }

        if (anim != null)
        {
            anim.SetBool("IsFiring", isFiring);
        }
    }

    IEnumerator DelayedFire(Vector2 targetPosition)
    {
        yield return new WaitForSeconds(2f); // **İlk girişte 2 saniye bekle**
        FireCanonBall(targetPosition);
        nextFireTime = Time.time + fireRate; 
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
