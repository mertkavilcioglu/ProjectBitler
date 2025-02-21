using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private Vector3 mousePos;

    [Header("References")]
    public GameObject cross;        
    public GameObject bullet;       
    public Transform firePoint;     

    [Header("Shooting Settings")]
    public float fireRate = 0.5f;   
    private float nextFireTime = 0f;
private bool isShooting = false;

AudioManager audioManager;

private void Awake()
{
    audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
}
    void Update()
    {
        
        mousePos = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)
        );
        
        
        cross.transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);

        
        Vector3 targetDirection = mousePos - transform.position;
        float rotateZ = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotateZ);

        
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate; 
            Shoot();
        }

        if(Input.GetMouseButton(0))
        {
            isShooting=true;
        }
        else
        {
            isShooting=false;
        }
    }

    private void Shoot()
    {
        if(isShooting==true)
        Instantiate(bullet, firePoint.position, transform.rotation);
        audioManager.PlaySFX(audioManager.shooting);// bu olmadı düzelticem -.-
        
    }
}



