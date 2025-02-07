using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject EnemyPrefab;

    [SerializeField]
    private float EnemyInterval = 5f;

    [SerializeField]
    private int MaxEnemyCount = 15;

    private List<GameObject> enemies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy(EnemyInterval, EnemyPrefab));
    }

    private IEnumerator spawnEnemy(float Interval, GameObject Enemy) {
        while (true)
        {
            yield return new WaitForSeconds(Interval);
            enemies.RemoveAll(enemy => enemy == null);
            if (enemies.Count < MaxEnemyCount)
            {
                GameObject newEnemy = Instantiate(Enemy, new Vector3(Random.Range(-5f, 5f), Random.Range(-6f, 6f), 0), Quaternion.identity);
                enemies.Add(newEnemy);
            }
        }
    }
}
