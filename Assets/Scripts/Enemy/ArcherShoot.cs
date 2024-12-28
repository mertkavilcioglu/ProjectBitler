using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherShoot : MonoBehaviour
{
    public GameObject arrow;
    public Transform arrowPos;

    private float timer;
    void Start()
    {
        
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 2)
        {
            timer = 0;
            shoot();
        }


    }

    void shoot()
    {
        Instantiate(arrow, arrowPos.position, Quaternion.identity);
    }
}
