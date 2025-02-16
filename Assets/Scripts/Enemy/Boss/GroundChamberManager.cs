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
    [SerializeField] private Color chamberColor = new Color(1f, 0f, 0f, 0.3f);

    [Header("Spawn Area")]
    [SerializeField] private Vector2 areaSize = new Vector2(28f, 10f);
    [SerializeField] private Vector2 areaOffset = Vector2.zero;

    [Header("References")]
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private int chamberDamage = 10;
    [SerializeField] private float damageTickRate = 0.5f;

    private bool isSpawning = false;
    private List<GameObject> activeChambers = new List<GameObject>();
    private float nextDamageTime;

    private void Start()
    {
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
            if (bossHealth.GetCurrentHealth() <= 500f)//////////////////////////////////////////////////////////////////////////
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
            // Remove any chambers that exceed the max limit
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
        Vector2 spawnPos = (Vector2)transform.position + areaOffset + randomOffset;

        GameObject chamber = CreateChamber(spawnPos);
        activeChambers.Add(chamber);

        StartCoroutine(DestroyChamberAfterDelay(chamber));
    }

    private GameObject CreateChamber(Vector2 position)
    {
        GameObject chamber = new GameObject("GroundChamber");
        chamber.transform.position = position;

        SpriteRenderer renderer = chamber.AddComponent<SpriteRenderer>();
        renderer.sprite = CreateCircleSprite();
        renderer.color = chamberColor;
        renderer.sortingOrder = -1;

        float scaleFactor = chamberRadius * 2f;
        chamber.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);

        CircleCollider2D collider = chamber.AddComponent<CircleCollider2D>();
        collider.radius = 0.5f;
        collider.isTrigger = true;

        ChamberDamage damageComponent = chamber.AddComponent<ChamberDamage>();
        damageComponent.Initialize(chamberDamage, damageTickRate);

        StartCoroutine(FadeIn(renderer));

        return chamber;
    }

    private IEnumerator FadeIn(SpriteRenderer renderer)
    {
        float elapsed = 0f;
        float duration = 0.5f;
        Color startColor = renderer.color;
        startColor.a = 0f;
        renderer.color = startColor;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, chamberColor.a, elapsed / duration);
            renderer.color = new Color(chamberColor.r, chamberColor.g, chamberColor.b, alpha);
            yield return null;
        }
    }

    private IEnumerator DestroyChamberAfterDelay(GameObject chamber)
    {
        yield return new WaitForSeconds(chamberLifetime - 0.5f);

        // Fade out
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

    private Sprite CreateCircleSprite()
    {
        int textureDiameter = 256;
        Texture2D texture = new Texture2D(textureDiameter, textureDiameter);

        float radius = textureDiameter / 2f;
        Color[] colors = new Color[textureDiameter * textureDiameter];

        for (int x = 0; x < textureDiameter; x++)
        {
            for (int y = 0; y < textureDiameter; y++)
            {
                float distance = Vector2.Distance(
                    new Vector2(x, y),
                    new Vector2(radius, radius)
                );

                if (distance < radius)
                {
                    colors[y * textureDiameter + x] = Color.white;
                }
                else
                {
                    colors[y * textureDiameter + x] = Color.clear;
                }
            }
        }

        texture.SetPixels(colors);
        texture.Apply();

        return Sprite.Create(
            texture,
            new Rect(0, 0, textureDiameter, textureDiameter),
            new Vector2(0.5f, 0.5f)
        );
    }

}