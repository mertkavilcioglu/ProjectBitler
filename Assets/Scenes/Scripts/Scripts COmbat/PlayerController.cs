using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

  public float moveSpeed = 5f;
public Rigidbody2D rb;
public Weapon weapon;

Vector2 moveDirection;
Vector2 mousePosition;

void Update()
{
    // Get input for horizontal and vertical movement
    float moveX = Input.GetAxisRaw("Horizontal");
    float moveY = Input.GetAxisRaw("Vertical");

    // Check for left mouse button click to fire weapon
    if (Input.GetMouseButtonDown(0))
    {
        weapon.Fire();
    }

    // Normalize movement direction to maintain consistent speed
    moveDirection = new Vector2(moveX, moveY).normalized;

    // Get mouse position in world coordinates
    mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
}

void FixedUpdate()
{
    // Apply movement to the Rigidbody2D
    rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
}


}
