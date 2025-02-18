using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ships : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform shipsPoint;
    [SerializeField] private RectTransform shipsLocator; // Değiştirildi

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (shipsPoint == null)
            shipsPoint = GameObject.Find("ShipsPoint")?.transform;

        if (shipsLocator == null)
            shipsLocator = GameObject.Find("ShipsLocator")?.GetComponent<RectTransform>(); // Değiştirildi
    }

    private void Update()
    {
        if (player == null || shipsLocator == null || shipsPoint == null) return;

        Vector2 direction = (shipsPoint.position - player.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Oku döndür (UI içinde olduğu için rotation kullanılıyor)
        shipsLocator.rotation = Quaternion.Euler(0, 0, angle);
        shipsLocator.anchoredPosition = Vector2.zero;
    }
}
