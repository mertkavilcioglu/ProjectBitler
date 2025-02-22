using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgiaSofia : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player; // Oyuncu referansı
    [SerializeField] private Transform agiaSofiaPoint; // Ayasofya hedefi
    [SerializeField] private RectTransform agiaSofiaLocator; // UI için yön oku

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (agiaSofiaPoint == null)
            agiaSofiaPoint = GameObject.Find("AgiaSofiaPoint")?.transform;

        if (agiaSofiaLocator == null)
            agiaSofiaLocator = GameObject.Find("AgiaSofiaLocator")?.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (player == null || agiaSofiaLocator == null || agiaSofiaPoint == null) return;

        Vector2 direction = (agiaSofiaPoint.position - player.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Oku döndür (UI için rotation kullanılıyor)
        agiaSofiaLocator.rotation = Quaternion.Euler(0, 0, angle - 41);
        agiaSofiaLocator.anchoredPosition = Vector2.zero;
    }
}
