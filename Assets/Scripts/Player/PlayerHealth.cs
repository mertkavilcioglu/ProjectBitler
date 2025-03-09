using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System.Collections;
public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;
    
    [Header("Health Bar UI")]
    public GameObject healthBarPrefab;
    public Vector3 healthBarOffset;
    private HealthBar healthBarInstance;
    public Canvas healthBarCanvas;
    public Animator animator;
    private string currentScene;
    AudioSource audioSource;
    
    public AudioClip deathFriend;
    
    public void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        currentHealth = maxHealth;
        
        if (currentScene == "Ayasofya_ic")
        {
            healthBarOffset = new Vector3(-6f, 16f, 0f);
        }
        
        CreateHealthBar();
    }
    
    private void CreateHealthBar()
    {
        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance.gameObject);
        }
        
        if (healthBarPrefab != null && healthBarCanvas != null)
        {
            GameObject hb = Instantiate(healthBarPrefab, healthBarCanvas.transform);
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
            Debug.LogWarning("Health bar prefab or canvas is missing!");
        }
    }
    
    private void Update()
    {
        currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("LastActiveScene", currentScene);
        PlayerPrefs.Save();
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
        if (!isDead)
        {
            isDead = true;
            audioSource.PlayOneShot(deathFriend);

            // Disable player controls immediately
            DisablePlayerControls();

            animator.SetBool("IsDead", true);
            StartCoroutine(HandleDeath());
        }
    }

    private void DisablePlayerControls()
    {
        var playerMovement = GetComponent<PlayerController>();
        if (playerMovement != null)
            playerMovement.enabled = false;

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = Vector2.zero;
    }

    private IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(1f);

        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance.gameObject);
        }

        SceneManager.LoadScene("YouDiedRetry");
    }
}