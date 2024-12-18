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
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        // Apply movement to the Rigidbody2D
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    // Yukseltme fonksiyonlari
    public void UpgradeHealth(float extraMaxHealth)
    {
        GetComponent<HealthManager>().IncreaseMaxHealth(extraMaxHealth);
    }

    public void UpgradeFireRate(float extraFireRate)
    {
        weapon.fireRate += extraFireRate;
    }

    public void UpgradeDamage(float extraDamage)
    {
        weapon.bulletDamage += extraDamage;
    }

    public void UpgradeWeaponLevel()
    {
        weapon.fireForce += 5f; // Silah seviyesiyle mermi gucunu artiriyoruz
    }
}