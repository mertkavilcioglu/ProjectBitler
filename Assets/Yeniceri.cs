using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yeniceri : MonoBehaviour
{
    public float detectionRange = 10f;
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    public int damage = 10;

    private Transform enemyPos;
    public float speed;
    private float lastAttackTime;
    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        enemyPos = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToEnemy = Vector2.Distance(transform.position, enemyPos.position);

        if (distanceToEnemy > attackRange)
        {
            isAttacking = false;
            transform.position = Vector2.MoveTowards(
                transform.position,
                enemyPos.position,
                speed * Time.deltaTime
            );
        }
        else
        {
            // Oyuncu saldýrý mesafesinde ve saldýrý soðuma süresi dolmuþsa saldýr
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
        Debug.Log("Yeniceri saldýrýyor!");

        // Oyuncuya hasar ver
        EnemyHealth enemyHealth = enemyPos.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }

        //Invoke("ResetAttack", attackAnimationLength);
    }
}
