using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    [Header("Enemy Spawning")]
    [SerializeField] private GameObject[] enemyPrefabs;    // Array of enemy prefabs to spawn
    [SerializeField] private Transform[] spawnPoints;      // Array of spawn points around the boss
    [SerializeField] private float spawnInterval = 7f;     // Time between spawns in seconds
    [SerializeField] private int enemiesPerWave = 3;      // Number of enemies to spawn each wave

    [SerializeField] private float detectionRange = 15f;   // Range at which boss notices player

    private Transform player;
    private bool isPlayerInRange;
    private bool canSpawnEnemies = true;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(SpawnEnemiesRoutine());
    }

    private void Update()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
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
            enemyMovement.BossSceneRange(newPlayerDetection: 10f);
        }
    }
    
}