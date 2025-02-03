using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyMovement : MonoBehaviour
{
    [Header("Detection & Attack Settings")]
    public float detectionRange = 10f;
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    public int damage = 1; // Melee enemy'nin vereceği hasar

    private Transform playerPos;
    public float speed;
    private float lastAttackTime;
    private bool isAttacking = false;
    Animator animator;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerPos.position);

        // Yüz yönünü oyuncuya doğru çevir
        if (playerPos.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        if (distanceToPlayer > attackRange)
        {
            // Oyuncu saldırı mesafesinde değilse enemy yürüyüş animasyonunu oynatsın
            isAttacking = false;
            transform.position = Vector2.MoveTowards(
                transform.position,
                playerPos.position,
                speed * Time.deltaTime
            );
            animator.Play("Base Layer.Enemy_Walk");
        }
        else
        {
            // Oyuncu saldırı mesafesinde ve saldırı soğuma süresi dolmuşsa saldır
            if (!isAttacking && Time.time - lastAttackTime > attackCooldown)
            {
                isAttacking = true;
                lastAttackTime = Time.time;
                AttackPlayer();
            }
        }
    }

    void AttackPlayer()
    {
        animator.Play("Base Layer.Enemy_Attack");
        Debug.Log("Enemy saldırıyor!");

        // Oyuncuya hasar ver
        PlayerHealth playerHealth = playerPos.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        // Saldırı animasyonu süresi kadar bekleyip saldır bayrağını resetle
        float attackAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        Invoke("ResetAttack", attackAnimationLength);
    }

    void ResetAttack()
    {
        isAttacking = false;
    }
}
