using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{
    public Transform player; 
    public GameObject[] staticPlatformPrefabs; 
    public GameObject movingPlatformPrefab;
    public float spawnDistance = 10f; 
    public float minY = 0.8f, maxY = 2f; 
    public float minX = -3f, maxX = 3f; 
    public float minZ = -3f, maxZ = 3f; 
    public int platformsAhead = 20; 
    [Range(0f, 1f)] public float staticPlatformChance = 0.7f; 

    public int easyPlatforms = 3; 
    public float easyPlatformY = 1.0f; 
    private int platformsSpawned = 0;

    public int platformsPerLevel = 2; 
    public float minHorizontalDistance = 1.2f; 
    public float minVerticalDistance = 0.5f; 

    private float highestY = 0f;

    void Start()
    {
        highestY = player.position.y;
        for (int i = 0; i < platformsAhead; i++)
        {
            SpawnNextPlatform();
        }
    }

    void Update()
    {
        if (player.position.y + spawnDistance > highestY)
        {
            SpawnNextPlatform();
        }
    }

    void SpawnNextPlatform()
    {
        float y;
        if (platformsSpawned < easyPlatforms)
            y = highestY + easyPlatformY;
        else
            y = highestY + Random.Range(minY, maxY);

        List<Vector3> usedPositions = new List<Vector3>();
        for (int i = 0; i < platformsPerLevel; i++)
        {
            GameObject prefab;
            float rand = Random.value;
            if (rand < staticPlatformChance)
                prefab = staticPlatformPrefabs[Random.Range(0, staticPlatformPrefabs.Length)];
            else
                prefab = movingPlatformPrefab;

            float x, z;
            int attempts = 0;
            Vector3 newPos;
            do
            {
                x = Random.Range(minX, maxX);
                z = Random.Range(minZ, maxZ);
                newPos = new Vector3(x, y, z);
                attempts++;
            } while (usedPositions.Exists(val => Vector3.Distance(val, newPos) < minHorizontalDistance || Mathf.Abs(val.y - newPos.y) < minVerticalDistance) && attempts < 10);

            usedPositions.Add(newPos);

            Instantiate(prefab, newPos, Quaternion.identity, transform);
        }

        highestY = y;
        platformsSpawned++;
    }
}