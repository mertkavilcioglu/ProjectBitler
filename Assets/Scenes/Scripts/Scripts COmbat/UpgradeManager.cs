using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public PlayerController player;
    public int upgradePoints = 0;
    private int enemiesKilled = 0;
    private int enemiesNeededForUpgrade = 10;

    public void ApplyHealthUpgrade()
    {
        if (upgradePoints > 0)
        {
            player.UpgradeHealth(20f); // Maksimum cani 20 artır
            SpendUpgradePoint();
        }
    }

    public void ApplyFireRateUpgrade()
    {
        if (upgradePoints > 0)
        {
            player.UpgradeFireRate(0.5f); // Ates hizini artir
            SpendUpgradePoint();
        }
    }

    public void ApplyDamageUpgrade()
    {
        if (upgradePoints > 0)
        {
            player.UpgradeDamage(5f); // Mermi hasarini artir
            SpendUpgradePoint();
        }
    }

    public void ApplyWeaponLevelUpgrade()
    {
        if (upgradePoints > 0)
        {
            player.UpgradeWeaponLevel(); // Silah seviyesini artir
            SpendUpgradePoint();
        }
    }

    // Bir düşman öldürüldüğünde çağrılacak
    public void OnEnemyKilled()
    {
        enemiesKilled++;
        CheckForUpgradePoints();
    }

    // Öldürülen düşman sayısına göre yükseltme puanı ekle
    private void CheckForUpgradePoints()
    {
        if (enemiesKilled >= enemiesNeededForUpgrade)
        {
            upgradePoints++;
            enemiesKilled -= enemiesNeededForUpgrade;
            enemiesNeededForUpgrade += 2; // Gereken düşman sayısını artır
            Debug.Log($"Upgrade Point Earned! Total Points: {upgradePoints}. Next Upgrade Requires: {enemiesNeededForUpgrade} Enemies.");
        }
    }

    // Yükseltme puanı harca
    private void SpendUpgradePoint()
    {
        upgradePoints--;
        Debug.Log($"Upgrade Point Spent! Remaining Points: {upgradePoints}.");
    }
}
