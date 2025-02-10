using UnityEngine;

public class CanonBall : MonoBehaviour
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
       
        if (rb.velocity == Vector2.zero)
        {
            rb.velocity = transform.right * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Çarpışma anında alan hasarını uygula.
        Explode();
        
        // Çarpışmadan sonra mermiyi yok et.
        Destroy(gameObject);
    }

    private void Explode()
    {
        // explosionRadius yarıçapı içindeki tüm colliderları bul.
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D hit in hits)
        {
            
            if (hit.CompareTag("Player"))
            {
                PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }
            }
            else if (hit.CompareTag("FriendSoldier"))
            {
                YeniceriHealth yeniceriHealth = hit.GetComponent<YeniceriHealth>();
                if (yeniceriHealth != null)
                {
                    yeniceriHealth.TakeDamage(damage);
                }
            }
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
