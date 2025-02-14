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

    [Header("Fire Chamber Settings")]
    [SerializeField] private GameObject fireChamberObject;
    [SerializeField] private float chamberRadius = 15f;
    [SerializeField] private int chamberDamage = 10;
    [SerializeField] private float damageTickRate = 0.5f;
    [SerializeField] private Color chamberColor = new Color(1f, 0f, 0f, 0.3f);

    private float lerpTimer;
    private float delayedTimer;
    private bool isFireChamberActive = false;
    private float nextDamageTime;
    private SpriteRenderer fireChamberRenderer;
    private CircleCollider2D fireChamberCollider;

    private void Start()
    {
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

        InitializeFireChamber();
        ShowHealthBar();
    }
    private void InitializeFireChamber()
    {
        if (fireChamberObject == null)
        {
            fireChamberObject = new GameObject("FireChamber");
            fireChamberObject.transform.parent = transform;
            fireChamberObject.transform.localPosition = Vector3.zero;

            fireChamberRenderer = fireChamberObject.AddComponent<SpriteRenderer>();
            fireChamberRenderer.sprite = CreateCircleSprite();
            fireChamberRenderer.color = chamberColor;
            fireChamberRenderer.sortingOrder = -1;

            fireChamberCollider = fireChamberObject.AddComponent<CircleCollider2D>();
            fireChamberCollider.radius = chamberRadius;
            fireChamberCollider.isTrigger = true;

            fireChamberObject.SetActive(false);
        }
    }

    private Sprite CreateCircleSprite()
    {
        int textureDiameter = 256;
        Texture2D texture = new Texture2D(textureDiameter, textureDiameter);

        float radius = textureDiameter / 2f;
        Color[] colors = new Color[textureDiameter * textureDiameter];

        for (int x = 0; x < textureDiameter; x++)
        {
            for (int y = 0; y < textureDiameter; y++)
            {
                float distance = Vector2.Distance(
                    new Vector2(x, y),
                    new Vector2(radius, radius)
                );

                if (distance < radius)
                {
                    colors[y * textureDiameter + x] = Color.white;
                }
                else
                {
                    colors[y * textureDiameter + x] = Color.clear;
                }
            }
        }

        texture.SetPixels(colors);
        texture.Apply();

        return Sprite.Create(
            texture,
            new Rect(0, 0, textureDiameter, textureDiameter),
            new Vector2(0.5f, 0.5f)
        );
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
                Debug.Log($"Delayed Health Slider Value: {delayedHealthSlider.value}");
            }
        }
        if (!isFireChamberActive && currentHealth <= maxHealth-10)
        {
            ActivateFireChamber();
        }
    }
    private void ActivateFireChamber()
    {
        isFireChamberActive = true;
        if (fireChamberObject != null)
        {
            fireChamberObject.SetActive(true);
            StartCoroutine(PulseChamberEffect());
        }
    }

    private IEnumerator PulseChamberEffect()
    {
        while (isFireChamberActive)
        {
            // Create a pulsing effect by modifying the alpha
            float baseAlpha = chamberColor.a;
            float pulseSpeed = 2f;

            // Pulse the alpha value between 0.2 and 0.4
            float newAlpha = baseAlpha + (Mathf.Sin(Time.time * pulseSpeed) * 0.1f);
            fireChamberRenderer.color = new Color(
                chamberColor.r,
                chamberColor.g,
                chamberColor.b,
                newAlpha
            );

            yield return null;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isFireChamberActive && other.CompareTag("Player") && Time.time >= nextDamageTime)
        {
            // Apply damage to player
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(chamberDamage);
                nextDamageTime = Time.time + damageTickRate;
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
        healthBarCanvas.alpha = 1f;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    private void Die()
    {
        if (fireChamberObject != null)
        {
            fireChamberObject.SetActive(false);
        }

        Debug.Log($"{gameObject.name} has been defeated!");
    }
}