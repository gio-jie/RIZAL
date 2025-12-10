using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs; // 3 item prefabs
    public Transform[] spawnPoints;  // assign all spawn points in inspector

    void Start()
    {
        SpawnItems();
    }

    void SpawnItems()
    {
        // Convert spawnPoints into a list so we can remove chosen points
        List<Transform> availablePoints = new List<Transform>(spawnPoints);

        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            // Pick a random point from available list
            int randomIndex = Random.Range(0, availablePoints.Count);
            Transform chosenPoint = availablePoints[randomIndex];

            // Spawn item
            Instantiate(itemPrefabs[i], chosenPoint.position, Quaternion.identity);

            // Remove used point so no duplicates
            availablePoints.RemoveAt(randomIndex);
        }
    }
}
