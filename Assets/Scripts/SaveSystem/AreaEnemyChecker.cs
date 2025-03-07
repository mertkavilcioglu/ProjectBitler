using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*debuglari silmeyelim ltfn
3 area cizilecek, idleri verilecek, mission scriptinde cagrilacak*/

public class AreaEnemyChecker : MonoBehaviour
{
    private HashSet<GameObject> enemiesInArea = new HashSet<GameObject>();
    public int areaID;
    private bool hasReportedCleared = false;

    private void Start()
    {
        BoxCollider2D areaCollider = GetComponent<BoxCollider2D>();
        if (areaCollider == null)
        {
            //Debug.LogError($"Area {areaID}: No BoxCollider2D found!");
            return;
        }

        Vector2 size = areaCollider.size * transform.localScale;
        Vector2 position = (Vector2)transform.position + areaCollider.offset;

        //Debug.Log($"Area {areaID}: Checking for enemies in area of size {size} at position {position}");

        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, size, 0);
        //Debug.Log($"Area {areaID}: Found {colliders.Length} total colliders in area");

        foreach (Collider2D collider in colliders)
        {
            Debug.Log($"Area {areaID}: Found object: {collider.gameObject.name} with tag: {collider.gameObject.tag}");
            if (collider.CompareTag("Enemy") || collider.CompareTag("Boss"))
            {
                enemiesInArea.Add(collider.gameObject);
                Debug.Log($"Area {areaID}: Added enemy: {collider.gameObject.name}");
            }
        }

        Debug.Log($"Area {areaID}: Initial enemy count: {enemiesInArea.Count}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Area {areaID}: OnTriggerEnter2D detected object: {other.gameObject.name} with tag: {other.gameObject.tag}");
        if (other.CompareTag("Enemy"))
        {
            enemiesInArea.Add(other.gameObject);
            hasReportedCleared = false;
            Debug.Log($"Area {areaID}: Enemy entered. Total enemies: {enemiesInArea.Count}");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInArea.Remove(other.gameObject);
            CheckEnemies();
            Debug.Log($"Area {areaID}: Enemy left or died. Total enemies: {enemiesInArea.Count}");
        }
    }

    private void Update()
    {
        bool enemyWasDestroyed = false;
        enemiesInArea.RemoveWhere(enemy =>
        {
            if (enemy == null)
            {
                enemyWasDestroyed = true;
                return true;
            }
            return false;
        });

        if (enemyWasDestroyed)
        {
            CheckEnemies();
        }
    }

    private void CheckEnemies()
    {
        if (enemiesInArea.Count == 0 && !hasReportedCleared)
        {
            Debug.Log($"Area {areaID}: All enemies eliminated!");
            hasReportedCleared = true;
        }
        else if (enemiesInArea.Count > 0)
        {
            Debug.Log($"Area {areaID}: {enemiesInArea.Count} enemies remaining");
        }
    }
    public bool IsAreaCleared()
    {
        return enemiesInArea.Count == 0;
    }
}
