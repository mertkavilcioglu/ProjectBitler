using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int areaID; //icinde oldugu area hangisiyse

    public int maxHealth = 100;
    private int currentHealth;
    private Animator animator;

    public GameObject healthBarPrefab;
   
    public Vector3 healthBarOffset;

    private HealthBar healthBarInstance;
    
    AudioSource audioSource;

    public GameObject bloodEffectPrefab;

    public void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();//yagiz???
    }
    
    public AudioClip deathSound;

    private void Start()
    {
        StartCoroutine(CheckMissionStatus());

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
                Debug.LogWarning("Sahnede Canvas yok ki canim");
            }
        }
    }

    private void Update()
    {

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        float fillAmount = (float)currentHealth / maxHealth;
        if (healthBarInstance != null)
        {
            healthBarInstance.SetFillAmount(fillAmount);
        }
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

    private void Die()
    {
        if (healthBarInstance != null)
        {
            audioSource.clip = deathSound;
            audioSource.Play();
            Destroy(healthBarInstance.gameObject);
        }

        for (int i = 0; i < 3; i++)
        {
            SpawnBloodEffect();
        }

        StartCoroutine(HandleDeath());
        
    }
    private IEnumerator HandleDeath()
    {
        
        animator.SetBool("IsDead", true);//ozge yapamamis?
        audioSource.clip = deathSound;
        audioSource.PlayOneShot(deathSound);
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
    private IEnumerator CheckMissionStatus()
    {
        yield return new WaitForSeconds(0.2f);

        if (MissionManager.Instance != null && MissionManager.Instance.IsAreaCompleted(areaID))
        {
            Debug.Log($"Enemy {gameObject.name} in completed area {areaID} - disabling");
            gameObject.SetActive(false);
            //healthBarInstance.gameObject.SetActive(false);
        }
    }
}
