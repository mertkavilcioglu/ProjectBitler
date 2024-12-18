using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image healthBar; // Saglik cubugu referansi
    public float healthAmount = 100f; // Mevcut can miktari
    public float maxHealth = 100f; // Maksimum can miktari

    void Start()
    {
        UpdateHealthBar(); // Saglik cubugunu baslangicta ayarla
    }

    void Update()
    {
        // Can sifir olursa sahneyi yeniden baslat
        if (healthAmount <= 0)
        {
            ReloadScene();
        }

        // Test icin: Enter tusuna basildiginda hasar al
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TakeDamage(20);
        }

        // Test icin: Space tusuna basildiginda iyiles
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Heal(5);
        }

        // Test icin: M tusuna basildiginda maksimum cani artir
        if (Input.GetKeyDown(KeyCode.M))
        {
            IncreaseMaxHealth(20);
        }
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage; // Can azalt
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealth); // Sinirla
        UpdateHealthBar(); // Saglik cubugunu guncelle
    }

    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount; // Can artir
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealth); // Sinirla
        UpdateHealthBar(); // Saglik cubugunu guncelle
    }

    public void IncreaseMaxHealth(float extraHealth)
    {
        maxHealth += extraHealth; // Maksimum cani artir
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealth); // Mevcut cani sinirla
        UpdateHealthBar(); // Saglik cubugunu guncelle
    }

    void ReloadScene()
    {
        // Sahneyi yeniden yukle
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }

    void UpdateHealthBar()
    {
        // Saglik cubugunu maksimum cana gore guncelle
        healthBar.fillAmount = healthAmount / maxHealth;
    }
}
