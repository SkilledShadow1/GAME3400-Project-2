using System;
using UnityEngine;

public class BoulderSpawner : MonoBehaviour
{
    [SerializeField] private float boulderSpawnRate;
    [SerializeField] private GameObject boulderPrefab;
    private float timeSinceLastSpawn;
    private void Start()
    {
        timeSinceLastSpawn = 0;
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= boulderSpawnRate) {
            Instantiate(boulderPrefab, transform.position, transform.rotation);
            timeSinceLastSpawn = 0;
        }
    }
    
    
}
