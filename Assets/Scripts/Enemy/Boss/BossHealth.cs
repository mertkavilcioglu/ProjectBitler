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
    private Transform player;
    Animator animator;

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
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (currentHealth <= maxHealth/2)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            animator.SetBool("IsWalking", true);
        }
    }

    public void TakeDamage(int damage)
    {
        delayedTimer = 0f;
        currentHealth = Mathf.Max(0, currentHealth - damage);
        if (currentHealth <= 0)
        {
            Die();
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
        Destroy(gameObject);        
    }

}