using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherShoot : MonoBehaviour
{
    public GameObject arrow;
    public Transform arrowPos;

    private float timer;
    
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
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
        audioManager.PlaySFX(audioManager.bow);
        Instantiate(arrow, arrowPos.position, Quaternion.identity);
    }
}
