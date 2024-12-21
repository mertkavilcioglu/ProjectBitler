using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Game Play")]
    public float speed;
    private Vector2 movement;
    public int playerHealth = 3;
    void Start()
    {
        
    }

    void Update()
    {    

        FlipCharacter();


        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(
            movement.x * speed,
            movement.y * speed
        );

        runAnim();
    }

    private void runAnim() {
        if(movement.x != 0 || movement.y != 0) {
            animator.SetBool("IsRunning", true);
        }
        else {
            animator.SetBool("IsRunning", false);
        }
    }

 void FlipCharacter()
    {
        
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        
        Vector3 playerPosition = transform.position;

        
        if (mousePosition.x > playerPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); 
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); 
        }
    }


}

