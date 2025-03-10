using UnityEngine;

public class ArcherEnemy : MonoBehaviour 
{
    public float playerDetectionRange = 8f;
    public float friendSoldierDetectionRange = 10f;
    
    public float shootRange = 10f;
    public float speed = 3f;
    
    [HideInInspector]
    public Transform currentTarget;
    
    private Transform playerTransform;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= playerDetectionRange)
        {
            currentTarget = playerTransform;
        }
        else
        {
            FindNearestFriendSoldier();
        }
        
        if (currentTarget == null)
        {
            animator.SetBool("IsRange", false);
            animator.SetBool("IsFiring", false);
            return;
        }
        else
        {
            animator.SetBool("IsRange", true);
        }
        
        float distanceToTarget = Vector2.Distance(transform.position, currentTarget.position);
        
        // Rotate towards target using rotation instead of scale
        FlipCharacter();
        
        if (distanceToTarget > shootRange)
        {
            animator.SetBool("IsFiring", false);
            transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("IsFiring", true);
        }
    }
    
    void FlipCharacter()
    {
        Vector3 targetPosition = currentTarget.position;
        Vector3 archerPosition = transform.position;
        
        if (targetPosition.x > archerPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);  // Değişti: 0 -> 180
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);    // Değişti: 180 -> 0
        }
    }
    
    void FindNearestFriendSoldier()
    {
        GameObject[] friendSoldiers = GameObject.FindGameObjectsWithTag("FriendSoldier");
        float nearestDistance = Mathf.Infinity;
        Transform nearestTarget = null;
        
        foreach (GameObject soldier in friendSoldiers)
        {
            float distance = Vector2.Distance(transform.position, soldier.transform.position);
            if (distance < nearestDistance && distance <= friendSoldierDetectionRange)
            {
                nearestDistance = distance;
                nearestTarget = soldier.transform;
            }
        }
        
        currentTarget = nearestTarget;
    }
}