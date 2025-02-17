using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;       
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.velocity = transform.right * speed;
    }

   private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
    {
        enemyHealth.TakeDamage(20); // Mermi 1 hasar veriyor
        Destroy(gameObject);         
    }
    else if (collision.gameObject.CompareTag("Boss"))
    {
        BossHealth bossHealth = collision.gameObject.GetComponent<BossHealth>();
        if (bossHealth != null)
        {
            bossHealth.TakeDamage(20);
        }
    }
    
    Destroy(gameObject);         
    
}

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

