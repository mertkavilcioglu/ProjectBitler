using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;  // Düşmanın maksimum canı
    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    
    
    
       public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " has taken " + damage + " damage.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

   
    // Düşman canı bittiğinde çağrılır.
    
    void Die()
    {
        // Buraya düşman öldüğünde oynatılacak animasyon
        
        Destroy(gameObject);
    }
}
