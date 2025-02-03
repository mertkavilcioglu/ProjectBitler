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
        enemyHealth.TakeDamage(1); // Mermi 1 hasar veriyor
        Destroy(gameObject);         
    }
    else if (!collision.gameObject.CompareTag("Player"))
    {
        Destroy(gameObject);         
    }
}

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

