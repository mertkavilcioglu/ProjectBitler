using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    [Header("Enemy Spawning")]
    [SerializeField] private GameObject[] enemyPrefabs;    // Array of enemy prefabs to spawn
    [SerializeField] private Transform[] spawnPoints;      // Array of spawn points around the boss
    [SerializeField] private float spawnInterval = 7f;     // Time between spawns in seconds
    [SerializeField] private int enemiesPerWave = 3;      // Number of enemies to spawn each wave

    [Header("Boss Attack")]
    [SerializeField] private float attackRange = 10f;      // Range at which boss can attack player
    [SerializeField] private float attackCooldown = 3f;    // Time between attacks
    [SerializeField] private int swordDamage = 25;      // Damage dealt by sword attack
    [SerializeField] private float detectionRange = 15f;   // Range at which boss notices player

    private Transform player;
    private bool isPlayerInRange;
    private float lastAttackTime;
    private bool canSpawnEnemies = true;

    private void Start()
    {
        // Find the player by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Start the enemy spawning coroutine
        StartCoroutine(SpawnEnemiesRoutine());
    }

    private void Update()
    {
        if (player == null) return;

        // Check if player is in range
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        isPlayerInRange = distanceToPlayer <= detectionRange;

        if (isPlayerInRange)
        {
            // Face the player
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Attack if in range and cooldown is over
            if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                PerformSwordAttack();
            }
        }
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

        // Randomly select an enemy prefab and spawn point
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }

    private void PerformSwordAttack()
    {
        // Create a line between boss and player to represent sword attack
        RaycastHit2D hit = Physics2D.Raycast(transform.position,
            (player.position - transform.position).normalized,
            attackRange);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            // Deal damage to player
            PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(swordDamage);
            }
        }

        // Update last attack time
        lastAttackTime = Time.time;
    }

    // Optional: Visualize the attack and detection ranges in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}