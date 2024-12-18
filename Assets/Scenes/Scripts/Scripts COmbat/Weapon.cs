using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce = 20f;

    public float fireRate = 1f; // Saldiri hizi (1 saniyede 1 atis)
    private float lastFireTime; // Son ates zamani

    public float bulletDamage = 10f; // Merminin verdigi hasar

    public void Fire()
    {
        if (Time.time - lastFireTime >= 1 / fireRate)
        {
            lastFireTime = Time.time;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // Hasari mermiye iletin
            bullet.GetComponent<Bullet>().damage = bulletDamage;

            bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
        }
    }
}
