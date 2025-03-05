using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundChamberManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 7f;
    [SerializeField] private float chamberLifetime = 5f;
    [SerializeField] private int maxChambers = 3;
    [SerializeField] private float chamberRadius = 2f;
    [SerializeField] Material bloom;

    [Header("Spawn Area")]
    [SerializeField] private Vector2 areaSize = new Vector2(28f, 10f);
    [SerializeField] private Vector2 areaOffset = Vector2.zero;

    [Header("References")]
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private int chamberDamage = 10;
    [SerializeField] private float damageTickRate = 0.5f;

    [SerializeField] private RuntimeAnimatorController chamberAnimator;
    private bool isSpawning = false;
    private List<GameObject> activeChambers = new List<GameObject>();
    private Transform playerPos;

    private void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (bossHealth == null)
        {
            bossHealth = FindObjectOfType<BossHealth>();
        }

        if (bossHealth != null)
        {
            StartCoroutine(CheckBossHealth());
        }
    }

    private IEnumerator CheckBossHealth()
    {
        bool spawningStarted = false;

        while (!spawningStarted)
        {
            if (bossHealth.GetCurrentHealth() <= 990f)//////////////////////////////////////////////////////////////////////////
            {
                StartSpawning();
                spawningStarted = true;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnChamberRoutine());
        }
    }

    private IEnumerator SpawnChamberRoutine()
    {
        while (isSpawning)
        {
            while (activeChambers.Count >= maxChambers)
            {
                if (activeChambers[0] != null)
                {
                    Destroy(activeChambers[0]);
                }
                activeChambers.RemoveAt(0);
            }

            SpawnChamber();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnChamber()
    {
        Vector2 randomOffset = new Vector2(
            Random.Range(-areaSize.x / 2, areaSize.x / 2),
            Random.Range(-areaSize.y / 2, areaSize.y / 2)
        );
        Vector2 spawnPos = (Vector2)playerPos.transform.position + areaOffset + randomOffset;

        GameObject chamber = CreateChamber(spawnPos);
        activeChambers.Add(chamber);

        StartCoroutine(DestroyChamberAfterDelay(chamber));
    }

    private GameObject CreateChamber(Vector2 position)
    {
        GameObject chamber = new GameObject("GroundChamber");
        chamber.transform.position = position;

        SpriteRenderer renderer = chamber.AddComponent<SpriteRenderer>();
        renderer.material = bloom;
        renderer.material.SetTexture("_BaseMap", null);

        renderer.sortingOrder = -1;

        // Add Animator component and set the controller
        Animator animator = chamber.AddComponent<Animator>();
        animator.runtimeAnimatorController = chamberAnimator;

        // Scale setup
        float scaleFactor = chamberRadius * 2f;
        chamber.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);

        CircleCollider2D collider = chamber.AddComponent<CircleCollider2D>();
        collider.radius = 0.5f;
        collider.isTrigger = true;

        ChamberDamage damageComponent = chamber.AddComponent<ChamberDamage>();
        damageComponent.Initialize(chamberDamage, damageTickRate);

        return chamber;
    }


    private IEnumerator DestroyChamberAfterDelay(GameObject chamber)
    {
        yield return new WaitForSeconds(chamberLifetime - 0.5f);

        if (chamber != null)
        {
            SpriteRenderer renderer = chamber.GetComponent<SpriteRenderer>();
            float elapsed = 0f;
            float duration = 0.5f;
            Color startColor = renderer.color;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(startColor.a, 0f, elapsed / duration);
                renderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                yield return null;
            }
        }

        if (chamber != null)
        {
            activeChambers.Remove(chamber);
            Destroy(chamber);
        }
    }

    

}