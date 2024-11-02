using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject collectablePrefab; 
    public float spawnRange = 5f;        // spawn range around spawner
    public int maxSpawns = 10;           

    private int currentSpawnCount = 0;  

    private void Start()
    {
        // initial spawning
        for (int i = 0; i < maxSpawns; i++)
        {
            SpawnCollectable();
        }
    }

    private void Update()
    {
        // check and spawn new collectables
        if (currentSpawnCount < maxSpawns)
        {
            SpawnCollectable();
        }
    }

    private void SpawnCollectable()
    {
        // random position for each spawn
        Vector3 spawnPosition = GetRandomSpawn();

        Collider collider = collectablePrefab.GetComponent<Collider>();
        if (collider != null)
        {
            spawnPosition.y += collider.bounds.extents.y;
        }

        // spawn
        Instantiate(collectablePrefab, spawnPosition, Quaternion.identity);
        currentSpawnCount++;
    }

    private Vector3 GetRandomSpawn()
    {
        float randomX = Random.Range(-spawnRange, spawnRange);
        float randomZ = Random.Range(-spawnRange, spawnRange);

        return new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    }
}
