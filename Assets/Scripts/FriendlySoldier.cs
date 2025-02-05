using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlySoldier : MonoBehaviour
{
    [Header("Combat Settings")]
    public float detectionRange = 10f; // Düşmanı algılama menzili
    public float attackRange = 1.5f;   // Saldırı menzili
    public float attackCooldown = 1f; // Saldırı soğuma süresi
    public int damage = 10;           // Saldırı hasarı

    private Transform targetEnemy;    // Hedef düşman
    public float speed = 2f;          // Hareket hızı
    private float lastAttackTime;     // Son saldırı zamanı
    private bool isAttacking = false;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        FindClosestEnemy(); // En yakın düşmanı bul

        if (targetEnemy != null)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, targetEnemy.position);

            // Yüz yönünü düşmana çevir
            spriteRenderer.flipX = targetEnemy.position.x < transform.position.x;

            if (distanceToEnemy > attackRange)
            {
                // Düşman saldırı menzilinde değilse hareket et
                isAttacking = false;
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    targetEnemy.position,
                    speed * Time.deltaTime
                );
                animator.Play("Base Layer.Friendly_Walk");
            }
            else
            {
                // Düşman saldırı menzilindeyse saldır
                if (!isAttacking && Time.time - lastAttackTime > attackCooldown)
                {
                    isAttacking = true;
                    lastAttackTime = Time.time;
                    AttackEnemy();
                }
            }
        }
    }

    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemySoldier"); // Düşman askerleri bul
        float closestDistance = detectionRange;
        targetEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                targetEnemy = enemy.transform;
            }
        }
    }

    void AttackEnemy()
    {
        animator.Play("Base Layer.Friendly_Attack");

        // Hedef düşmana hasar ver
        //Enemy enemyHealth = targetEnemy.GetComponent<Enemy>();
       // if (enemyHealth != null)
       // {
        //    enemyHealth.TakeDamage(damage);
       // }

        // Saldırı animasyonu süresi kadar bekleyip saldır bayrağını sıfırla
        float attackAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        Invoke("ResetAttack", attackAnimationLength);
    }

    void ResetAttack()
    {
        isAttacking = false;
    }
}
