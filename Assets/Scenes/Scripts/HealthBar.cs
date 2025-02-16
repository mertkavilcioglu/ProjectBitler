using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Tooltip("Can barındaki doldurma görseli (Fill).")]
    public Image fillImage;

    [Tooltip("Hedef objeye göre ofset (örneğin, objenin üstünde görünmesi için).")]
    public Vector3 offset;

    private Transform target;
    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    
    // Health bar’ın takip edeceği hedefi ayarlar.
        public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

   
    public void SetFillAmount(float amount)
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = amount;
        }
    }

    private void Update()
    {
        if (target != null)
        {
            // Hedefin world konumunu, Canvas’ın ekran konumuna çevir.
            Vector3 screenPos = mainCam.WorldToScreenPoint(target.position + offset);
            transform.position = screenPos;
        }
    }
}
