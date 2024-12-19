using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;  
    public Transform firePoint;      
    public float fireForce = 2f;    
    public float fireRate = 0.5f;    

    private float nextFireTime = 0.5f; 

    public void Fire()
    {
        
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;  

            
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            
            Vector2 fireDirection = (mousePosition - (Vector2)firePoint.position).normalized;

            
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            
            float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            
            bullet.GetComponent<Rigidbody2D>().AddForce(fireDirection * fireForce, ForceMode2D.Impulse);
        }
    }
}




