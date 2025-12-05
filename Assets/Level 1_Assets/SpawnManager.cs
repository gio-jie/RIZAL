using UnityEngine;
using System.Collections.Generic; // Required to use Lists

public class SpawnManager : MonoBehaviour
{
    [Header("What to Spawn")]
    public GameObject taskObjectPrefab; // Drag your Yellow Square Prefab here

    [Header("Where to Spawn")]
    public Transform[] spawnPoints;     // Drag your Point1, Point2, etc. here

    [Header("Settings")]
    public int amountToSpawn = 3;       // How many items do you want?

    void Start()
    {
        SpawnRandomObjects();
    }

    void SpawnRandomObjects()
    {
        // 1. Create a temporary list of all possible spots
        // (We use a list so we can remove spots as we use them)
        List<Transform> availableSpots = new List<Transform>(spawnPoints);

        // 2. Loop 3 times (or however many items you want)
        for (int i = 0; i < amountToSpawn; i++)
        {
            // Safety check: If we run out of spots, stop trying to spawn
            if (availableSpots.Count == 0) return;

            // 3. Pick a random number between 0 and the number of spots left
            int randomIndex = Random.Range(0, availableSpots.Count);

            // 4. Get the position of that random spot
            Transform selectedSpot = availableSpots[randomIndex];

            // 5. Create the Task Object at that position
            Instantiate(taskObjectPrefab, selectedSpot.position, Quaternion.identity);

            // 6. Remove this spot from the list so we don't pick it again
            availableSpots.RemoveAt(randomIndex);
        }
    }
}