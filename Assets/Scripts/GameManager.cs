using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float initialSpawnRate = 3f;
    [SerializeField] private float spawnRateReduction = 0.1f; // Amount to reduce spawn rate
    [SerializeField] private float minimumSpawnRate = 0.5f;
    [SerializeField] private float spawnRadius = 50f; // Radius for spawning enemies

    [Header("Pools")]
    [SerializeField] private Transform enemyPool;
    [SerializeField] private Transform activeEnemyPool;
    [SerializeField] private Transform player; // Reference to the player's position

    private float _currentSpawnRate;
    private float _spawnTimer;

    private void OnEnable()
    {
        HiddenExpManager.OnSpawnRateIncreased += IncreaseSpawnRate;
    }

    private void OnDisable()
    {
        HiddenExpManager.OnSpawnRateIncreased -= IncreaseSpawnRate;
    }

    private void Start()
    {
        _currentSpawnRate = initialSpawnRate;
        _spawnTimer = _currentSpawnRate;

        // Automatically find the player if not assigned in the Inspector
        if (player == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform; // Assign the player's Transform
            }
            else
            {
                Debug.LogError("Player GameObject not found! Ensure the player is tagged as 'Player'.");
            }
        }
    }

    private void Update()
    {
        _spawnTimer -= Time.deltaTime;

        if (_spawnTimer <= 0f)
        {
            SpawnEnemyNearPlayer();
            _spawnTimer = _currentSpawnRate;
        }
    }

    private void SpawnEnemyNearPlayer()
    {
        // Generate a random position near the player
        Vector2 randomPosition = GetRandomPositionAroundPlayer(player.position, spawnRadius);

        // Get a valid NavMesh position
        Vector3 validPosition = GetValidSpawnPosition(randomPosition);

        Transform enemy;

        if (enemyPool.childCount > 0)
        {
            // Get an enemy from the pool
            enemy = enemyPool.GetChild(0);
            enemy.SetParent(activeEnemyPool);
            enemy.position = validPosition;
            enemy.gameObject.SetActive(true);
        }
        else
        {
            // Instantiate a new enemy if the pool is empty
            GameObject newEnemy = Instantiate(enemyPrefab, validPosition, Quaternion.identity);
            enemy = newEnemy.transform;
            enemy.SetParent(activeEnemyPool); // Add it to the active pool
        }
    }

    private Vector2 GetRandomPositionAroundPlayer(Vector2 playerPosition, float radius)
    {
        // Generate a random offset within a circle
        Vector2 randomOffset = Random.insideUnitCircle * radius;

        // Add the random offset to the player's position
        return playerPosition + randomOffset;
    }

    private Vector3 GetValidSpawnPosition(Vector2 desiredPosition, float maxDistance = 10f)
    {
        NavMeshHit hit;

        // Check if the desired position is on or near the NavMesh
        if (NavMesh.SamplePosition(new Vector3(desiredPosition.x, desiredPosition.y, 0f), out hit, maxDistance, NavMesh.AllAreas))
        {
            return hit.position; // Return the nearest valid position
        }

        // If no valid position is found, try a random position within the NavMesh bounds
        Debug.LogWarning("No valid NavMesh position found near the desired position. Finding random NavMesh position...");
        return GetRandomPositionOnNavMesh();
    }

    private Vector3 GetRandomPositionOnNavMesh()
    {
        // Find a random point within the NavMesh bounds
        Bounds navMeshBounds = GetNavMeshBounds();
        for (int i = 0; i < 100; i++) // Try up to 100 times
        {
            Vector3 randomPoint = new Vector3(
                Random.Range(navMeshBounds.min.x, navMeshBounds.max.x),
                Random.Range(navMeshBounds.min.y, navMeshBounds.max.y),
                0f
            );

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1f, NavMesh.AllAreas))
            {
                return hit.position; // Return the valid position
            }
        }

        // If no position is found after multiple attempts, log an error
        Debug.LogError("Could not find a valid NavMesh position after multiple attempts.");
        return Vector3.zero;
    }

    private Bounds GetNavMeshBounds()
    {
        // Calculate the bounds of the NavMesh
        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
        Vector3 min = triangulation.vertices[0];
        Vector3 max = triangulation.vertices[0];

        foreach (Vector3 vertex in triangulation.vertices)
        {
            min = Vector3.Min(min, vertex);
            max = Vector3.Max(max, vertex);
        }

        return new Bounds((min + max) / 2, max - min);
    }

    private void IncreaseSpawnRate()
    {
        _currentSpawnRate -= spawnRateReduction;
        if (_currentSpawnRate < minimumSpawnRate)
        {
            _currentSpawnRate = minimumSpawnRate;
        }

        Debug.Log($"Spawn rate updated: {_currentSpawnRate} seconds");
    }
}
