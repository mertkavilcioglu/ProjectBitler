using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Weapon weaponLeft;  
    public Weapon weaponRight; 

    Vector2 moveDirection;
    Vector2 mousePosition;

    private float nextFireTime = 0.5f; 

    void Update()
    {
        
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        
        moveDirection = new Vector2(moveX, moveY).normalized;

        
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 0.5f; 

            
            weaponRight.Fire();

            
            weaponLeft.Fire();
        }
    }

    void FixedUpdate()
    {
        
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }
}

