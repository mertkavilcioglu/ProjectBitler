using UnityEngine;

public class YeniceriHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Health Bar UI")]
    public GameObject healthBarPrefab;
    public Vector3 healthBarOffset;

    private HealthBar healthBarInstance;
    
    AudioSource audioSource;

    public void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        currentHealth = maxHealth;
        

    }
    
    public AudioClip deathSound;
    

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


        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (healthBarInstance != null)
        {
            audioSource.clip = deathSound;
            audioSource.PlayOneShot(deathSound);
            Destroy(healthBarInstance.gameObject);
        }
        Destroy(gameObject);
    }
}
