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
    public Animator animator;
    public string currentScene;


    AudioSource audioSource;

    public void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        
    }

    public AudioClip deathFriend;

    private void Start()
    {
        currentHealth = maxHealth;
        if (healthBarPrefab != null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                GameObject hb = Instantiate(healthBarPrefab, canvas.transform);
                Canvas healthBarCanvas = hb.GetComponent<Canvas>();
                if (healthBarCanvas != null)
                {
                    healthBarCanvas.sortingOrder = 1;
                }

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
                Debug.LogWarning("Sahnede Canvas bulunamadi!");
            }
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
            animator.SetBool("IsDead", true);
            StartCoroutine(HandleDeath());
        }
        
    }
    private IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);

        SceneManager.LoadScene("YouDiedRetry");
    }

}