using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player; // Oyuncu referansı
    [SerializeField] private RectTransform compassUI; // UI içindeki pusula resmi

    private Vector3 lastPosition;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Oyuncu hareket ettiyse yön değiştir
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}