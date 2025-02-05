using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class ArcherEnemy : MonoBehaviour
{
    public float detectionRange = 1f;
    public float attackRange = 1f;
    public float attackCooldown = 1f;

    private Transform playerPos;
    public float speed;
    private float lastAttackTime;
    private bool isInAttackRange = false;
    Animator animator;
    private bool isAttacking = false;
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

        if (distanceToPlayer > attackRange)
        {
            isInAttackRange = false;
            transform.position = Vector2.MoveTowards(
                transform.position,
                playerPos.position,
                speed * Time.deltaTime
            );
            animator.Play("Base Layer.enemy2_walk");
        }
        else
        {
            isInAttackRange = true;
            StartAttack();
        }

        void StartAttack()
        {
            animator.Play("Base Layer.enemy2_attack");
            Debug.Log("Enemy is attacking!");

            // Reset attacking flag after animation length
            float attackAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;
            Invoke("ResetAttack", attackAnimationLength);
        }

        void ResetAttack()
        {
            isAttacking = false;
        }
    }
}
