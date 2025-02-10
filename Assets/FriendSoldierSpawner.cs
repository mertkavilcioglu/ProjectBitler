using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendSoldierSpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject FriendSoldierPrefab;

    [SerializeField]
    private float FriendSoldierInterval = 3f;

    [SerializeField]
    private int MaxFriendSoldierCount = 15;

    private List<GameObject> friendsoldiers = new List<GameObject>();

    public GameObject friendSoldierSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnFriendSoldier(FriendSoldierInterval, FriendSoldierPrefab));
    }

    private IEnumerator spawnFriendSoldier(float Interval, GameObject FriendSoldier) {
        while (true)
        {
            yield return new WaitForSeconds(Interval);
            friendsoldiers.RemoveAll(friendsoldier => friendsoldier == null);
            if (friendsoldiers.Count < MaxFriendSoldierCount)
            {
                //Vector3 spawnPosition = GetSpawnPosition();
                GameObject newFriendSoldier = Instantiate(FriendSoldier, friendSoldierSpawnPoint.transform.position, Quaternion.identity);
                friendsoldiers.Add(newFriendSoldier);
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
        float spawnX = cam.transform.position.x + cameraWidth / 2 - 1f; // Sol sınırın hemen dışı
        float spawnY = Random.Range(cam.transform.position.y - cameraHeight / 2, cam.transform.position.y + cameraHeight / 2); // Kamera yüksekliğinde rastgele bir y değeri

        return new Vector3(spawnX, spawnY, 0); // Düzlemde spawn pozisyonu
    }
}
