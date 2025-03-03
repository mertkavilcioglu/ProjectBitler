using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{

    public int maxHealth = 100;
    private int currentHealth;
    private Animator animator;

    public GameObject healthBarPrefab;
   
    public Vector3 healthBarOffset;

    private HealthBar healthBarInstance;
    
    AudioSource audioSource;
    
    public void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();//yagiz???
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
                Debug.LogWarning("Sahnede Canvas yog!");
            }
        }
    }

    private void Update()
    {

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteRenderer.flipX = player.transform.position.x > transform.position.x;
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
            audioSource.clip = deathSound;
            audioSource.Play();
            Destroy(healthBarInstance.gameObject);
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
}
