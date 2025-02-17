using UnityEngine;

public class DamageChamber : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float damageTickRate = 0.5f;
    private float nextDamageTime;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time >= nextDamageTime)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                nextDamageTime = Time.time + damageTickRate;
            }
        }
    }
}