using UnityEngine;
using System.Collections.Generic;

public class GuardSpawnManager : MonoBehaviour
{
    public List<Transform> spawnPoints; // assign same points as patrol points
    public GameObject guardPrefab;

    void Start()
    {
        if (spawnPoints.Count == 0 || guardPrefab == null) return;

        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Instantiate(guardPrefab, spawn.position, Quaternion.identity);
    }
}
