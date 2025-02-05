using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeFriendlyMovement : MonoBehaviour
{
    [Header("Detection & Attack Settings")]
    public float detectionRange = 10f;
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    public int damage = 10; // Dost birimin vereceği hasar

    private Transform targetEnemy;
    public float speed;
    private float lastAttackTime;
    private bool isAttacking = false;
    Animator animator;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // En yakın düşmanı tespit et
        FindClosestEnemy();

        if (targetEnemy == null) return;

        float distanceToEnemy = Vector2.Distance(transform.position, targetEnemy.position);

        // Yüz yönünü düşmana doğru çevir
        if (targetEnemy.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        if (distanceToEnemy > attackRange)
        {
            // Düşman saldırı mesafesinde değilse dost birim yürüsün
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
            // Düşman saldırı mesafesinde ve saldırı soğuma süresi dolmuşsa saldır
            if (!isAttacking && Time.time - lastAttackTime > attackCooldown)
            {
                isAttacking = true;
                lastAttackTime = Time.time;
                AttackEnemy();
            }
        }
    }

    void AttackEnemy()
    {
        animator.Play("Base Layer.Friendly_Attack");
        Debug.Log("Friendly saldırıyor!");

        // Düşmana hasar ver
        EnemyHealth enemyHealth = targetEnemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }

        // Saldırı animasyonu süresi kadar bekleyip saldır bayrağını resetle
        float attackAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        Invoke("ResetAttack", attackAnimationLength);
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= detectionRange)
            {
                shortestDistance = distanceToEnemy;
                closestEnemy = enemy.transform;
            }
        }

        targetEnemy = closestEnemy;
    }
}
