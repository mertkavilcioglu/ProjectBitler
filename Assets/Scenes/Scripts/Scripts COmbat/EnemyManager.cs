using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int enemiesKilled = 0; // Toplam oldurülen dusman sayisi
    public int upgradePoints = 0; // Kazanilan upgrade puanlari
    public int killThreshold = 10; // Ilk yukseltme icin gereken dusman sayisi

    // Dusman olduruldugunde cagrilacak fonksiyon
    public void OnEnemyKilled()
    {
        enemiesKilled++;

        // Eger yeterince dusman oldurulmusse upgrade puani kazanilir
        if (enemiesKilled >= killThreshold)
        {
            upgradePoints++;
            enemiesKilled = 0; // Sayaci sifirla
            killThreshold += 2; // Her seviyede dusman gereksinimini artir

            Debug.Log("Upgrade Point Earned! Current Points: " + upgradePoints);
            Debug.Log("Next Threshold: " + killThreshold + " enemies");
        }
    }
}
