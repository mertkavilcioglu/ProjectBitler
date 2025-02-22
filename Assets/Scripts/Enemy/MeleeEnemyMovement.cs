using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyMovement : MonoBehaviour
{
    [Header("Detection & Attack Settings")]
    public float playerDetectionRange = 5f;
    public float yeniceriDetectionRange = 30f;
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    public int damage = 10;

    private Transform playerPos;
    private Transform currentTarget;
    public float speed;
    private float lastAttackTime;
    private bool isAttacking = false;
    Animator animator;
    
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (playerPos == null || playerPos.Equals(null))
        {
            // Eğer oyuncu yok edilmişse, işlem yapma.
            return;
        }

        // First check if player is in range (priority target)
        float distanceToPlayer = Vector2.Distance(transform.position, playerPos.position);

        if (distanceToPlayer <= playerDetectionRange)
        {
            currentTarget = playerPos;
        }
        else
        {
            // Player not in range, look for Yeniceri
            FindNearestYeniceri();
        }

        if (currentTarget == null || currentTarget.Equals(null)) {
            return;
        }

        // Düşmanın hedefe doğru dönmesi
        FlipCharacter();

        float distanceToTarget = Vector2.Distance(transform.position, currentTarget.position);
        if (distanceToTarget > attackRange)
        {
            isAttacking = false;

            // Calculate position on a circle around the target
            float angle = Time.time * speed + transform.GetInstanceID(); // Unique for each unit
            float circleRadius = attackRange + 0.5f; // Slightly larger than attack range
            Vector2 offset = new Vector2(
                Mathf.Cos(angle) * circleRadius,
                Mathf.Sin(angle) * circleRadius
            );

            Vector2 targetPosition = (Vector2)currentTarget.position + offset;

            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );
            animator.Play("Base Layer.enemy1_walk");
        }
        else
        {
            if (!isAttacking && Time.time - lastAttackTime > attackCooldown)
            {
                isAttacking = true;
                lastAttackTime = Time.time;
                AttackTarget();
            }
        }
    }

    void FlipCharacter()
    {
        Vector3 targetPosition = currentTarget.position;
        Vector3 enemyPosition = transform.position;
        
        if (targetPosition.x > enemyPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void FindNearestYeniceri()
    {
        GameObject[] yeniceris = GameObject.FindGameObjectsWithTag("FriendSoldier");
        float nearestDistance = Mathf.Infinity;
        Transform nearestYeniceri = null;

        foreach (GameObject yeniceri in yeniceris)
        {
            if (yeniceri == null) continue; // Eğer nesne yoksa atla.

            float distance = Vector2.Distance(transform.position, yeniceri.transform.position);
            if (distance < nearestDistance && distance <= yeniceriDetectionRange)
            {
                nearestDistance = distance;
                nearestYeniceri = yeniceri.transform;
            }
        }

        currentTarget = nearestYeniceri;
    }

    void AttackTarget()
    {
        animator.Play("Base Layer.enemy1_attack");
        //Debug.Log($"Enemy saldırıyor: {currentTarget.name}!");
        audioManager.PlaySFX(audioManager.swordEnemy);

        if (currentTarget == playerPos)
        {
            PlayerHealth playerHealth = currentTarget.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
        else
        {
            YeniceriHealth yeniceriHealth = currentTarget.GetComponent<YeniceriHealth>();
            if (yeniceriHealth != null)
            {
                yeniceriHealth.TakeDamage(damage);
            }
        }

        float attackAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        Invoke("ResetAttack", attackAnimationLength);
    }

    void ResetAttack()
    {
        isAttacking = false;
    }
    
    public void BossSceneRange(float newPlayerDetection)
    {
        playerDetectionRange = newPlayerDetection;
    }
}