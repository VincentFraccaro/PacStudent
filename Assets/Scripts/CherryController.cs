using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    public GameObject cookiePrefab;
    public float spawnDelay = 10.0f;
    public float moveSpeed = 5.0f;

    private bool isSpawningCookie = false;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        InvokeRepeating("TrySpawnCookie", 0, spawnDelay);
    }

    void TrySpawnCookie()
    {
        if (!isSpawningCookie)
        {
            StartCoroutine(SpawnCookie());
        }
    }

    IEnumerator SpawnCookie()
    {
        isSpawningCookie = true;

        Vector3 spawnPosition = GetRandomSpawnLocation();
        GameObject newCookie = Instantiate(cookiePrefab, spawnPosition, Quaternion.identity);
        newCookie.tag = "Cookie";
        CircleCollider2D cl = newCookie.AddComponent<CircleCollider2D>();
        cl.isTrigger = true;

        yield return MoveCookie(newCookie);

        isSpawningCookie = false;
    }

    IEnumerator MoveCookie(GameObject cookie)
    {
        Vector3 startPosition = cookie.transform.position;
        Vector3 endPosition = new Vector3(27.0f, startPosition.y, 0.0f);
        float journeyLength = Vector3.Distance(startPosition, endPosition);
        float journeyTime = journeyLength / moveSpeed;
        float startTime = Time.time;

        while (Time.time - startTime < journeyTime)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            cookie.transform.position = Vector3.Lerp(startPosition, endPosition, fractionOfJourney);
            yield return null;
        }

        Destroy(cookie);
    }

    Vector3 GetRandomSpawnLocation()
    {
        // Determine a random spawn position outside of the camera view
        float spawnX, spawnY;

        int randomSide = Random.Range(0, 4);

        switch (randomSide)
        {
            case 0: // Left side
                spawnX = mainCamera.ViewportToWorldPoint(new Vector3(0, Random.value, 0)).x - 1.0f;
                spawnY = Random.Range(-2.0f, 2.0f);
                break;
            case 1: // Right side
                spawnX = mainCamera.ViewportToWorldPoint(new Vector3(1, Random.value, 0)).x + 1.0f;
                spawnY = Random.Range(-2.0f, 2.0f);
                break;
            case 2: // Top side
                spawnX = Random.Range(-2.0f, 2.0f);
                spawnY = mainCamera.ViewportToWorldPoint(new Vector3(Random.value, 1, 0)).y + 1.0f;
                break;
            case 3: // Bottom side
                spawnX = Random.Range(-2.0f, 2.0f);
                spawnY = mainCamera.ViewportToWorldPoint(new Vector3(Random.value, 0, 0)).y - 1.0f;
                break;
            default:
                spawnX = 0.0f;
                spawnY = 0.0f;
                break;
        }

        return new Vector3(spawnX, spawnY, 0.0f);
    }
}
