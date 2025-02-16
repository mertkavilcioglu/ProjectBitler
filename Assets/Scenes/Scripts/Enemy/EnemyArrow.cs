using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    public float force;
    public int damage = 15;
    private Rigidbody2D rb;
    private Transform target;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // Get archer's current target
        ArcherEnemy archer = transform.parent?.GetComponent<ArcherEnemy>();
        if (archer != null && archer.currentTarget != null)
        {
            target = archer.currentTarget;
        }

        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            rb.velocity = direction.normalized * force;
            float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot + 90);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
        else if (collision.gameObject.CompareTag("FriendSoldier"))
        {
            YeniceriHealth yeniceriHealth = collision.gameObject.GetComponent<YeniceriHealth>();
            if (yeniceriHealth != null)
            {
                yeniceriHealth.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}