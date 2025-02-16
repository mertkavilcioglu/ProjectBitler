using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform castleLocator;
    [SerializeField] private Transform castlePoint;
    // Start is called before the first frame update
    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (castlePoint == null)
            castlePoint = GameObject.Find("CastlePoint")?.transform;

        if (castleLocator == null)
            castleLocator = GameObject.Find("CastleLocator")?.GetComponent<Transform>();
    }

    private void Update()
    {
        if (player == null || castleLocator == null || castlePoint == null) return;

        Vector2 direction = (castlePoint.position - player.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Oku döndür (Canvas içinde olduğu için localEulerAngles kullanılıyor)
        castleLocator.localEulerAngles = new Vector3(0, 0, angle + 17);
    }
}