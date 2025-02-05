using UnityEngine;

public class CanonBall : MonoBehaviour
{
    
    public int damage = 10;

    public float speed = 10f;
    
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
    
        if(rb.velocity == Vector2.zero)
        {
            rb.velocity = transform.right * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if(playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
        
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        
        Destroy(gameObject);
    }
}
