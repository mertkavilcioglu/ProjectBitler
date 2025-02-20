using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    [Header("Enemy Spawning")]
    [SerializeField] private GameObject[] enemyPrefabs;    // Array of enemy prefabs to spawn
    [SerializeField] private Transform[] spawnPoints;      // Array of spawn points around the boss
    [SerializeField] private float spawnInterval;
    [SerializeField] private int enemiesPerWave = 3;

    [SerializeField] private float detectionRange;

    private Transform player;
    Animator animator;
    private bool isPlayerInRange;
    private bool canSpawnEnemies = true;
    
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(SpawnBoss);
    }

    public AudioClip SpawnBoss;
    public AudioClip DeathBoss;
    
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
        
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        

        while (canSpawnEnemies)
        {
            if (isPlayerInRange)
            {
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