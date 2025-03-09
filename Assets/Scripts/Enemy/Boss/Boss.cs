using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    [Header("Enemy Spawning")]
    [SerializeField] private GameObject[] enemyPrefabs;    // Array of enemy prefabs to spawn
    [SerializeField] private Transform[] spawnPoints;      // Array of spawn points around the boss
    [SerializeField] public float spawnInterval;
    [SerializeField] public int enemiesPerWave;

    [SerializeField] private float detectionRange;

    private Transform player;
    Animator animator;
    private bool isPlayerInRange;
    private bool canSpawnEnemies = true;
    
    AudioSource audioSource;

    public AudioClip bossBackground;
    public AudioClip bossScream;

    public void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = bossBackground;
        audioSource.Play();
    }

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(SpawnEnemiesRoutine());
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        isPlayerInRange = distanceToPlayer <= detectionRange;

        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        while (canSpawnEnemies)
        {
            if (isPlayerInRange)
            {
                audioSource.PlayOneShot(bossScream);
                animator.SetBool("IsSummoning", true);
                yield return new WaitForSeconds(1f);
                animator.SetBool("IsSummoning", false);

                for (int i = 0; i < enemiesPerWave; i++)
                {
                    SpawnEnemy();
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0 || spawnPoints.Length == 0) return;

        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        MeleeEnemyMovement enemyMovement = spawnedEnemy.GetComponent<MeleeEnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.BossSceneRange(newPlayerDetection: 25f);
        }
    }
}