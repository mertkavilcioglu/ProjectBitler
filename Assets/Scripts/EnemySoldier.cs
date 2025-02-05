using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoldier : MonoBehaviour
{
    [Header("Combat Settings")]
    public float detectionRange = 10f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    public int damage = 10;

    private Transform targetFriendly;
    public float speed = 2f;
    private float lastAttackTime;
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
        FindClosestFriendly();

        if (targetFriendly != null)
        {
            float distanceToFriendly = Vector2.Distance(transform.position, targetFriendly.position);

            spriteRenderer.flipX = targetFriendly.position.x < transform.position.x;

            if (distanceToFriendly > attackRange)
            {
                isAttacking = false;
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    targetFriendly.position,
                    speed * Time.deltaTime
                );
                animator.Play("Base Layer.Enemy_Walk");
            }
            else
            {
                if (!isAttacking && Time.time - lastAttackTime > attackCooldown)
                {
                    isAttacking = true;
                    lastAttackTime = Time.time;
                    AttackFriendly();
                }
            }
        }
    }

    void FindClosestFriendly()
    {
        GameObject[] friendlies = GameObject.FindGameObjectsWithTag("FriendlySoldier");
        float closestDistance = detectionRange;
        targetFriendly = null;

        foreach (GameObject friendly in friendlies)
        {
            float distance = Vector2.Distance(transform.position, friendly.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                targetFriendly = friendly.transform;
            }
        }
    }

    void AttackFriendly()
    {
        animator.Play("Base Layer.Enemy_Attack");

        FriendlySoldier friendlyHealth = targetFriendly.GetComponent<FriendlySoldier>();
        if (friendlyHealth != null)
        {
            friendlyHealth.TakeDamage(damage);
        }

        float attackAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        Invoke("ResetAttack", attackAnimationLength);
    }

    void ResetAttack()
    {
        isAttacking = false;
    }
}
