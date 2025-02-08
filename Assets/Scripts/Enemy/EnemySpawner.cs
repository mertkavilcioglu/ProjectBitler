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

    public GameObject spawnPoint;

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
                //Vector3 spawnPosition = GetSpawnPosition();
                GameObject newEnemy = Instantiate(Enemy, spawnPoint.transform.position, Quaternion.identity);
                enemies.Add(newEnemy);
            }
        }
    }

    private Vector3 GetSpawnPosition()
    {
        // Ana kameranın sınırlarını al
        Camera cam = Camera.main;
        if (cam == null) return Vector3.zero;

        // Kamera genişliği ve yüksekliği hesaplanır
        float cameraHeight = 2f * cam.orthographicSize;
        float cameraWidth = cameraHeight * cam.aspect;

        // Sağ sınırın biraz dışında bir x pozisyonu belirle
        float spawnX = cam.transform.position.x + cameraWidth / 2 + 1f; // Sağ sınırın hemen dışı
        float spawnY = Random.Range(cam.transform.position.y - cameraHeight / 2, cam.transform.position.y + cameraHeight / 2); // Kamera yüksekliğinde rastgele bir y değeri

        return new Vector3(spawnX, spawnY, 0); // Düzlemde spawn pozisyonu
    }
}
