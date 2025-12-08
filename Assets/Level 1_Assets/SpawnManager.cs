using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [Header("Specific Items to Spawn")]
    // Drag Quinine, Leaves, and Bandages prefabs here
    public GameObject[] uniqueItems;

    [Header("Locations")]
    public Transform[] spawnPoints;

    void Start()
    {
        SpawnUniqueItems();
    }

    void SpawnUniqueItems()
    {
        List<Transform> availableSpots = new List<Transform>(spawnPoints);

        // Loop through each unique item (Quinine, Leaves, Bandages)
        foreach (GameObject itemPrefab in uniqueItems)
        {
            if (availableSpots.Count == 0) return;

            // Pick a random spot
            int randomIndex = Random.Range(0, availableSpots.Count);
            Transform spot = availableSpots[randomIndex];

            // Create the specific item
            Instantiate(itemPrefab, spot.position, Quaternion.identity);

            // Remove spot so items don't overlap
            availableSpots.RemoveAt(randomIndex);
        }
    }
}