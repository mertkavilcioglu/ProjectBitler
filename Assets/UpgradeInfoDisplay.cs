using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeInfoDisplay : MonoBehaviour
{
    public EnemyManager enemyManager; // EnemyManager referansi
    public Text upgradeInfoText; // UI Text bileseni

    void Update()
    {
        // UI'da mevcut durumu goster
        upgradeInfoText.text = "Enemies to next upgrade: " + (enemyManager.killThreshold - enemyManager.enemiesKilled);
        upgradeInfoText.text += "\nUpgrade Points: " + enemyManager.upgradePoints;
    }
}
