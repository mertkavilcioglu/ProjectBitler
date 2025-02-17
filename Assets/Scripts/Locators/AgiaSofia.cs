using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgiaSofia : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player; // Oyuncu referansı
    [SerializeField] private Transform agiaSofiaLocator; // Pusula içindeki yön oku
    [SerializeField] private Transform agiaSofiaPoint; // Ayasofya hedefi

    private void Start()
    {
        // Oyuncu ve hedefi sahnede otomatik bul
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (agiaSofiaPoint == null)
            agiaSofiaPoint = GameObject.Find("AgiaSofiaPoint")?.transform;

        if (agiaSofiaLocator == null)
            agiaSofiaLocator = GameObject.Find("AgiaSofiaLocator")?.GetComponent<Transform>();
    }

    private void Update()
    {
        if (player == null || agiaSofiaLocator == null || agiaSofiaPoint == null) return;

        // Oyuncudan hedefe doğru yönü hesapla
        Vector2 direction = (agiaSofiaPoint.position - player.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Oku döndür (Canvas içinde olduğu için localEulerAngles kullanılıyor)
        agiaSofiaLocator.localEulerAngles = new Vector3(0, 0, angle - 41);
    }
}
