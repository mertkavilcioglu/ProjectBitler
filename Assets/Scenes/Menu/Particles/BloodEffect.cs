using UnityEngine;

public class BloodEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem bloodParticleSystem;
    [SerializeField] private float destroyDelay = 2f; // Time before the effect is destroyed

    private void Start()
    {
        // Destroy the effect after delay
        Destroy(gameObject, destroyDelay);
    }

    // Call this method when an enemy is hit
    public static void SpawnBlood(Vector2 position, Vector2 hitDirection)
    {
        // Instantiate the blood effect prefab at the hit position
        GameObject bloodEffect = Instantiate(Resources.Load<GameObject>("BloodEffect"), position, Quaternion.identity);

        // Rotate the blood effect to spray in the opposite direction of the hit
        float angle = Mathf.Atan2(hitDirection.y, hitDirection.x) * Mathf.Rad2Deg;
        bloodEffect.transform.rotation = Quaternion.Euler(0, 0, angle + 180);
    }
}