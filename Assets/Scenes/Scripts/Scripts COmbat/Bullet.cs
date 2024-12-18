using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage; // Merminin verdigi hasar

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Dusmana hasar ver
            collision.gameObject.GetComponent<HealthManager>().TakeDamage(damage);
        }

        Destroy(gameObject); // Mermiyi yok et
    }
}