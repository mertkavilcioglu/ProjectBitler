using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Health Bar UI")]
    
    public GameObject healthBarPrefab;
    
    public Vector3 healthBarOffset;

    private HealthBar healthBarInstance;
    public Animator animator;

    private void Start()
    {
        currentHealth = maxHealth;
        // Sahnedeki Canvas’ı bulup health bar prefab’ını oraya instantiate ediyoruz.
        if (healthBarPrefab != null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                GameObject hb = Instantiate(healthBarPrefab, canvas.transform);
                healthBarInstance = hb.GetComponent<HealthBar>();
                if (healthBarInstance != null)
                {
                    healthBarInstance.SetTarget(transform);
                    healthBarInstance.offset = healthBarOffset;
                    healthBarInstance.SetFillAmount(1f); // Tam dolu başlangıç
                }
            }
            else
            {
                Debug.LogWarning("Sahnede Canvas bulunamadı!");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        float fillAmount = (float)currentHealth / maxHealth;
        if (healthBarInstance != null)
        {
            healthBarInstance.SetFillAmount(fillAmount);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance.gameObject);
        }

        animator.SetBool("IsDead", true);

        Destroy(gameObject);
    }
}
