using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yeniceri : MonoBehaviour 
{
    public float detectionRange = 10f;
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    public int damage = 10;
    public Animator animator;
    private Transform currentTarget;
    public float speed;
    private float lastAttackTime;
    private bool isAttacking = false;
    AudioSource audioSource;

    public void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
       
    }
    
    public AudioClip swordFriend;

    void Update()
    {
        // Find nearest enemy if we don't have a target or current target is destroyed
        if (currentTarget == null)
        {
            FindNearestEnemy();
            // Reset animation if no target found
            if (currentTarget == null)
            {
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsAttacking", false);
                isAttacking = false;
                return;
            }
        }

        float distanceToEnemy = Vector2.Distance(transform.position, currentTarget.position);

        // Check if enemy is within detection range
        if (distanceToEnemy > detectionRange)
        {
            // Enemy too far, look for a closer one
            FindNearestEnemy();
            return;
        }

        // Düşmana doğru dön
        FlipCharacter();

        if (distanceToEnemy > attackRange)
        {
            isAttacking = false;
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsWalking", true);
            transform.position = Vector2.MoveTowards(
                transform.position,
                currentTarget.position,
                speed * Time.deltaTime
            );
        }
        else
        {
            if (!isAttacking && Time.time - lastAttackTime > attackCooldown)
            {
                animator.SetBool("IsWalking", false);
                isAttacking = true;
                lastAttackTime = Time.time;
                AttackEnemy();
            }
        }
    }

    void FlipCharacter()
    {
        if (currentTarget == null) return;

        Vector3 targetPosition = currentTarget.position;
        Vector3 yeniceriPosition = transform.position;
        
        if (targetPosition.x > yeniceriPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float nearestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        currentTarget = nearestEnemy?.transform;
    }

    void AttackEnemy()
    {
        if (currentTarget == null) return;
        audioSource.PlayOneShot(swordFriend);


        Debug.Log("Yeniceri saldırıyor!");
        animator.SetBool("IsAttacking", true);

        EnemyHealth enemyHealth = currentTarget.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }

        // Add this to reset the attack state
        StartCoroutine(ResetAttackState());
    }

    IEnumerator ResetAttackState()
    {
        // Wait for attack animation to complete
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.SetBool("IsAttacking", false);
        isAttacking = false;
    }
}