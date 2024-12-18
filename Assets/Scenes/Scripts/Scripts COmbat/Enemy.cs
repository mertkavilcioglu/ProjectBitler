using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyManager enemyManager; // EnemyManager referansi

    private void OnDestroy()
    {
        // EnemyManager'a dusmanin oldugunu bildir
        if (enemyManager != null)
        {
            enemyManager.OnEnemyKilled();
        }
    }
}
