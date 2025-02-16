using UnityEngine;

public class ChamberDamage : MonoBehaviour
{
    private int damage;
    private float tickRate;
    private float nextDamageTime;

    public void Initialize(int damageAmount, float damageTickRate)
    {
        damage = damageAmount;
        tickRate = damageTickRate;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time >= nextDamageTime)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                nextDamageTime = Time.time + tickRate;
            }
        }
    }
}