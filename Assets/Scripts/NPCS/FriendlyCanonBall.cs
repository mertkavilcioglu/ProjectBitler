using UnityEngine;

public class FriendlyCanonBall : MonoBehaviour
{

    public int damage = 10;           
    public float speed = 10f;         

    public float explosionRadius = 2f;  

    private Rigidbody2D rb;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Eğer mermiye henüz hız verilmediyse, mermiyi ileri doğru hareket ettir.
        if (rb.velocity == Vector2.zero)
        {
            rb.velocity = transform.right * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();
        Destroy(gameObject);
    }

    private void Explode()
    {
        // explosionRadius içindeki tüm colliderları al.
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D hit in hits)
        {
            // Eğer hedef Enemy tag’ına sahipse:
            if (hit.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                }
            }
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    
   
}
