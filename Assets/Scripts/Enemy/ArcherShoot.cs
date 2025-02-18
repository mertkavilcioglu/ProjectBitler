using UnityEngine;

public class ArcherShoot : MonoBehaviour
{
    public GameObject arrow;
    public Transform arrowPos;
    public float shootInterval = 2f; 

    private ArcherEnemy archerEnemy;
    private float timer;

    void Start()
    {
        archerEnemy = GetComponent<ArcherEnemy>();
    }

    void Update()
    {
        
        if (archerEnemy.currentTarget == null)
        {
            timer = 0;
            return;
        }

       
        float distanceToTarget = Vector2.Distance(transform.position, archerEnemy.currentTarget.position);

        
        if (distanceToTarget > archerEnemy.shootRange)
        {
            timer = 0;
            return;
        }

        
        timer += Time.deltaTime;

        
        if (timer >= shootInterval)
        {
            timer = 0f;
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(arrow, arrowPos.position, Quaternion.identity);
    }
}
