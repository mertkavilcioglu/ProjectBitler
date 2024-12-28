using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shooting : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;  // "mousPos" yerine doğru yazım: "mousePos"

    // Start is called before the first frame update
    void Start()
    {
        // Kamera objesini almak için Camera.main kullanıyoruz, bu daha güvenilir.
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Mouse pozisyonunu doğru şekilde alıyoruz.
        // Z değerini, kameranın nearClipPlane değeri ile ayarlıyoruz.
        mousePos = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCam.nearClipPlane));
        
        // Mouse pozisyonuyla karakter arasındaki farkı hesaplıyoruz.
        Vector3 direction = mousePos - transform.position;

        // Rotasyonu hesaplıyoruz.
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Karakterin rotasını ayarlıyoruz (Quaternion.Euler yerine Quaternion doğru yazım ile kullanılıyor).
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}