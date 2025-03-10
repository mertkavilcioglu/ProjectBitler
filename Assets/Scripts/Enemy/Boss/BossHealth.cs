using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    public float speed = 1f;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 1000;
    [SerializeField] private int currentHealth;
    public GameObject bloodEffectPrefab;

    [Header("UI Elements")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider delayedHealthSlider;
    [SerializeField] private TextMeshProUGUI bossNameText;
    [SerializeField] private CanvasGroup healthBarCanvas;

    [Header("Visual Settings")]
    [SerializeField] private float healthBarSpeed = 2f;
    [SerializeField] private float delayedBarSpeed = 0.5f;
    [SerializeField] private float fadeInDuration = 1f;

    [Header("Fire Chamber Settings")]
    [SerializeField] private GameObject damageZone;
    [SerializeField] private float healthThreshold;
    
    private float delayedTimer;
    private bool isDead = false;
    private Transform player;
    Animator animator;
    AudioSource audioSource;
    

    public void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    
    public AudioClip bossDeathSound;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        damageZone.SetActive(false);

        StartCoroutine(CheckHealth());

        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }

        if (delayedHealthSlider != null)
        {
            delayedHealthSlider.maxValue = maxHealth;
            delayedHealthSlider.value = maxHealth;
        }

        if (bossNameText != null)
        {
            bossNameText.text = "Constantine XI Palaiologos";
        }

        if (healthBarCanvas != null)
        {
            healthBarCanvas.alpha = 0f;
        }
        ShowHealthBar();
    }
    private IEnumerator CheckHealth()
    {
        while (true)
        {
            if (currentHealth <= healthThreshold)
            {
                damageZone.SetActive(true);
                break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Update()
    {
        if (isDead)
        {
            UpdateHealthBars();
            return;
        }

        UpdateHealthBars();
        
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (currentHealth <= maxHealth/2)//////////////////////////////////////////////////////////////////////////////////////////////
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            animator.SetBool("IsWalking", true);
        }
    }
    private void UpdateHealthBars()
    {
        if (healthSlider != null && healthSlider.value != currentHealth)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, currentHealth, Time.deltaTime * healthBarSpeed);
        }

        if (delayedHealthSlider != null && delayedHealthSlider.value > healthSlider.value)
        {
            delayedTimer += Time.deltaTime;
            if (delayedTimer >= delayedBarSpeed)
            {
                delayedHealthSlider.value = Mathf.Lerp(delayedHealthSlider.value, currentHealth, Time.deltaTime * healthBarSpeed);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        delayedTimer = 0f;
        currentHealth = Mathf.Max(0, currentHealth - damage);
        SpawnBloodEffect();
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void SpawnBloodEffect()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector2 hitDirection = (transform.position - player.transform.position).normalized;

        if (bloodEffectPrefab != null)
        {
            GameObject bloodEffect = Instantiate(bloodEffectPrefab, transform.position, Quaternion.identity);

            ParticleSystem ps = bloodEffect.GetComponentInChildren<ParticleSystem>();
            if (ps != null)
            {
                float angle = Mathf.Atan2(hitDirection.y, hitDirection.x) * Mathf.Rad2Deg;
                bloodEffect.transform.rotation = Quaternion.Euler(0, 0, angle + 180);
            }

            Destroy(bloodEffect, 2f);
        }
        else
        {
            try
            {
                BloodEffect.SpawnBlood((Vector2)transform.position, hitDirection);
            }
            catch (System.Exception)
            {
                Debug.LogWarning("BloodEffect script not found or Resources/BloodEffect prefab missing. Please assign a bloodEffectPrefab in the Inspector.");
            }
        }
    }

    private void ShowHealthBar()
    {
        if (healthBarCanvas != null)
        {
            StartCoroutine(FadeInHealthBar());
        }
    }

    private IEnumerator FadeInHealthBar()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            healthBarCanvas.alpha = newAlpha;
            yield return null;
        }
        healthBarCanvas.alpha = 1f;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    void Die()
    {
        isDead = true;
        speed = 0f;
        animator.SetBool("IsDead", true);
        audioSource.PlayOneShot(bossDeathSound);
        for (int i = 0; i < 3; i++)
        {
            SpawnBloodEffect();
        }
        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(2.03f);
        Destroy(gameObject);
        SceneManager.LoadScene("Credits");
    }

}