using UnityEngine;

public class ArcherEnemy : MonoBehaviour
{
    public float playerDetectionRange = 8f;
    public float yeniceriDetectionRange = 10f;
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    private Transform playerPos;
    public Transform currentTarget;
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

        if (distanceToPlayer <= playerDetectionRange)
        {
            currentTarget = playerPos;
        }
        else
        {
            FindNearestYeniceri();
        }

        if (currentTarget == null) return;

        float distanceToTarget = Vector2.Distance(transform.position, currentTarget.position);
        if (distanceToTarget > attackRange)
        {
            isInAttackRange = false;
            transform.position = Vector2.MoveTowards(
                transform.position,
                currentTarget.position,
                speed * Time.deltaTime
            );
            animator.Play("Base Layer.enemy2_walk");
        }
        else
        {
            isInAttackRange = true;
            StartAttack();
        }
    }

    void FindNearestYeniceri()
    {
        GameObject[] yeniceris = GameObject.FindGameObjectsWithTag("Friendly");
        float nearestDistance = Mathf.Infinity;
        Transform nearestYeniceri = null;

        foreach (GameObject yeniceri in yeniceris)
        {
            float distance = Vector2.Distance(transform.position, yeniceri.transform.position);
            if (distance < nearestDistance && distance <= yeniceriDetectionRange)
            {
                nearestDistance = distance;
                nearestYeniceri = yeniceri.transform;
            }
        }

        currentTarget = nearestYeniceri;
    }

    void StartAttack()
    {
        animator.Play("Base Layer.enemy2_attack");
        Debug.Log("Enemy is attacking!");
        float attackAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        Invoke("ResetAttack", attackAnimationLength);
    }

    void ResetAttack()
    {
        isAttacking = false;
    }
}