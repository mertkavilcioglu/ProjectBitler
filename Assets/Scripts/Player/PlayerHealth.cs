using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("Oyuncunun maksimum can değeri (Inspector üzerinden değiştirilebilir).")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Health Bar UI")]
    [Tooltip("Health bar prefab’ı (Border, Background, Fill içeren prefab).")]
    public GameObject healthBarPrefab;
    [Tooltip("Health bar’ın oyuncuya göre ofseti (örneğin, (0, 1, 0))")]
    public Vector3 healthBarOffset;

    private HealthBar healthBarInstance;

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
        Destroy(gameObject);
    }
}
