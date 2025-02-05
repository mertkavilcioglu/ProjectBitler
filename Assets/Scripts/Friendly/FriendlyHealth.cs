using UnityEngine;

public class FriendlyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("Dost birimin maksimum canı (Inspector üzerinden değiştirilebilir).")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Health Bar UI")]
    [Tooltip("Health bar prefab’ı (Border, Background, Fill içeren prefab).")]
    public GameObject healthBarPrefab;
    [Tooltip("Health bar’ın dost birime göre ofseti (örneğin, (0, 1, 0))")]
    public Vector3 healthBarOffset;

    private HealthBar healthBarInstance;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
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
                    healthBarInstance.SetFillAmount(1f);
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

        Debug.Log(gameObject.name + " " + damage + " hasar aldı.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        float fillAmount = (float)currentHealth / maxHealth;
        if (healthBarInstance != null)
        {
            healthBarInstance.SetFillAmount(fillAmount);
        }

        Debug.Log(gameObject.name + " " + healAmount + " kadar iyileştirildi.");
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
