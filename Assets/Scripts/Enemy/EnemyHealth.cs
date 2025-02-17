using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public int maxHealth = 100;
    private int currentHealth;

  
    public GameObject healthBarPrefab;
   
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
                Debug.LogWarning("Sahnede Canvas yog!");
            }
        }
    }

    private void Update()
    {
        if (Freezer.Instance.IsGameFrozen)
        {
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = player.transform.position.x > transform.position.x;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        float fillAmount = (float)currentHealth / maxHealth;
        if (healthBarInstance != null)
        {
            healthBarInstance.SetFillAmount(fillAmount);
        }

        Debug.Log(gameObject.name + " " + damage + " vurdu gol oldu.");

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
