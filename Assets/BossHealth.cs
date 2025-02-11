using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BossHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 1000;
    [SerializeField] private int currentHealth;

    [Header("UI Elements")]
    [SerializeField] private Slider healthSlider;             // The main health bar
    [SerializeField] private Slider delayedHealthSlider;      // Delayed bar that shows damage taken
    [SerializeField] private TextMeshProUGUI bossNameText;    // Boss name display
    [SerializeField] private CanvasGroup healthBarCanvas;     // To fade the health bar in/out

    [Header("Visual Settings")]
    [SerializeField] private float healthBarSpeed = 2f;       // Speed of health bar updates
    [SerializeField] private float delayedBarSpeed = 0.5f;    // Speed of delayed bar
    [SerializeField] private float fadeInDuration = 1f;       // How long it takes for the bar to appear

    private float lerpTimer;
    private float delayedTimer;

    private void Start()
    {
        // Initialize health
        currentHealth = maxHealth;

        // Setup UI elements
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

        // Set boss name
        if (bossNameText != null)
        {
            bossNameText.text = "Constantine XI Palaiologos";
        }

        // Start with health bar hidden
        if (healthBarCanvas != null)
        {
            healthBarCanvas.alpha = 0f;
        }

        // Show health bar when battle starts
        ShowHealthBar();
    }

    private void Update()
    {
        // Update the health bar smoothly
        if (healthSlider != null && healthSlider.value != currentHealth)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, currentHealth, Time.deltaTime * healthBarSpeed);
        }

        // Update the delayed health bar
        if (delayedHealthSlider != null && delayedHealthSlider.value > healthSlider.value)
        {
            delayedTimer += Time.deltaTime;
            if (delayedTimer >= delayedBarSpeed)
            {
                delayedHealthSlider.value = Mathf.Lerp(delayedHealthSlider.value, currentHealth, Time.deltaTime * healthBarSpeed);
                Debug.Log($"Delayed Health Slider Value: {delayedHealthSlider.value}");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"Taking {damage} damage!");
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
        healthBarCanvas.alpha = 1f; // Ensure we end at exactly 1
    }

    private void Die()
    {
        // Handle boss death here
        Debug.Log($"{gameObject.name} has been defeated!");
        // Add your death effects, animations, or game over logic
    }
}