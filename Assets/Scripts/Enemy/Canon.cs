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
    AudioSource audioSource;

    public void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        

    }

    public AudioClip cannon;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Oyuncu ve müttefik askerleri bul
        GameObject[] playerObjs = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] friendSoldierObjs = GameObject.FindGameObjectsWithTag("FriendSoldier");

        // İki diziyi tek bir diziye birleştir
        GameObject[] targets = playerObjs.Concat(friendSoldierObjs).ToArray();

        // En yakın hedefi bul
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

        // En yakın hedef menzil içindeyse
        if (nearestTarget != null && nearestDistance <= range)
        {
            isFiring = true;

            // Topu hedefin X konumuna göre flip yap (Y ekseni etrafında döndür)
            FlipCanon(nearestTarget.transform);

            // İlk kez menzile girdiyse 2 saniye gecikmeli ateş
            if (firstTimeInRange)
            {
                firstTimeInRange = false;
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
            firstTimeInRange = true; 
        }

        
        if (anim != null)
        {
            anim.SetBool("IsFiring", isFiring);
        }
    }

    IEnumerator DelayedFire(Vector2 targetPosition)
    {
        yield return new WaitForSeconds(2f);
        FireCanonBall(targetPosition);
        nextFireTime = Time.time + fireRate;
    }

    void FireCanonBall(Vector2 targetPosition)
    {
        if (canonBallPrefab != null && firePoint != null)
        {
            GameObject ball = Instantiate(canonBallPrefab, firePoint.position, Quaternion.identity);
            audioSource.PlayOneShot(cannon);

            
            Vector2 direction = (targetPosition - (Vector2)firePoint.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            ball.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

   
    private void FlipCanon(Transform target)
    {
        
        if (target.position.x >= transform.position.x)
        {
            
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }
}
