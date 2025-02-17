using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ships : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform shipsLocator;
    [SerializeField] private Transform shipsPoint;

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (shipsPoint == null)
            shipsPoint = GameObject.Find("ShipsPoint")?.transform;

        if (shipsLocator == null)
            shipsLocator = GameObject.Find("ShipsLocator")?.GetComponent<Transform>();
    }

    private void Update()
    {
        if (player == null || shipsLocator == null || shipsPoint == null) return;

        Vector2 direction = (shipsPoint.position - player.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Oku döndür (Canvas içinde olduğu için localEulerAngles kullanılıyor)
        shipsLocator.localEulerAngles = new Vector3(0, 0, angle + 9);
    }
}