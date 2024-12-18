using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f; // Hareket hızı
    public Rigidbody2D rigid; // Rigidbody2D bileşeni
    public Animator animator; // Animator bileşeni (isteğe bağlı)

    private Vector2 movement;

    void Update()
    {
        // WASD girişlerini al
        movement = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
            movement.y = 1;
        if (Input.GetKey(KeyCode.S))
            movement.y = -1;
        if (Input.GetKey(KeyCode.A))
            movement.x = -1;
        if (Input.GetKey(KeyCode.D))
            movement.x = 1;
        
        movement = movement.normalized;  // Normalize işlemi doğru şekilde yapılacak

        // Animator ayarları (isteğe bağlı)
        if (animator != null)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }
    }

    void FixedUpdate()
    {
        // Hareketi uygula
        rigid.MovePosition(rigid.position + movement * moveSpeed * Time.fixedDeltaTime); // movement.normalized yerine movement kullandım, zaten normalize edilmiş durumda
    }
}