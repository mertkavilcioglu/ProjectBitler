using UnityEngine;

public class BloodEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem bloodParticleSystem;
    [SerializeField] private float destroyDelay = 2f;

    private void Start()
    {
        Destroy(gameObject, destroyDelay);
    }

    public static void SpawnBlood(Vector2 position, Vector2 hitDirection)
    {
        GameObject bloodEffect = Instantiate(Resources.Load<GameObject>("BloodEffect"), position, Quaternion.identity);

        float angle = Mathf.Atan2(hitDirection.y, hitDirection.x) * Mathf.Rad2Deg;
        bloodEffect.transform.rotation = Quaternion.Euler(0, 0, angle + 180);
    }
}