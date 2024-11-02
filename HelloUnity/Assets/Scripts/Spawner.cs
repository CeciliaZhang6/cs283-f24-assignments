using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject collectablePrefab; // Template for the collectable
    public float spawnRange = 5f;        // Spawn range around the spawner
    public int maxSpawns = 10;           // Maximum number of collectables to have in the scene

    private int currentSpawnCount = 0;   // Tracks the number of currently active collectables

    private void Start()
    {
        // Initial spawning of collectable objects
        for (int i = 0; i < maxSpawns; i++)
        {
            SpawnCollectable();
        }
    }

    private void Update()
    {
        // Continuously check and spawn new collectables up to maxSpawns
        if (currentSpawnCount < maxSpawns)
        {
            SpawnCollectable();
        }
    }

    private void SpawnCollectable()
    {
        // Generate a new random position within the range for each spawn
        Vector3 spawnPosition = GetRandomPositionWithinRange();

        // Adjust position based on collider bounds to sit on terrain
        Collider collider = collectablePrefab.GetComponent<Collider>();
        if (collider != null)
        {
            spawnPosition.y += collider.bounds.extents.y;
        }

        // Instantiate a new collectable object at the computed position
        Instantiate(collectablePrefab, spawnPosition, Quaternion.identity);
        currentSpawnCount++;
    }

    private Vector3 GetRandomPositionWithinRange()
    {
        // Generate random X and Z positions within the specified range
        float randomX = Random.Range(-spawnRange, spawnRange);
        float randomZ = Random.Range(-spawnRange, spawnRange);

        // Calculate the final position using the spawner's position as the center
        return new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    }
}
