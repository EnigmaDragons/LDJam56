using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using DunGen;
using Unity.AI.Navigation;

public class EnemyTileSpawner : MonoBehaviour
{
    [SerializeField] private int minEnemies = 6;
    [SerializeField] private int maxEnemies = 20;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private BoxCollider safeZone;

    private Tile currentTile;
    private NavMeshSurface navMeshSurface;

    private void Start()
    {
        currentTile = GetComponent<Tile>();
        if (currentTile == null)
        {
            Debug.LogError("EnemyTileSpawner must be attached to a Tile object.");
            return;
        }

        navMeshSurface = GetComponentInChildren<NavMeshSurface>();
        if (navMeshSurface == null)
        {
            Debug.LogError("NavMeshSurface not found in the scene.");
            return;
        }

        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        int enemyCount = Random.Range(minEnemies, maxEnemies + 1);

        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPosition = GetValidSpawnPosition();
            if (spawnPosition != Vector3.zero)
            {
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, transform);
            }
        }
    }

    private Vector3 GetValidSpawnPosition()
    {
        for (int attempts = 0; attempts < 30; attempts++)
        {
            Vector3 randomPoint = GetRandomPointInBounds(currentTile.Bounds);
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                if (!Physics.CheckSphere(hit.position, 0.5f, LayerMask.GetMask("Obstacle")) && !IsInSafeZone(hit.position))
                {
                    return hit.position;
                }
            }
        }

        Debug.LogWarning("Could not find a valid spawn position after 30 attempts.");
        return Vector3.zero;
    }

    private Vector3 GetRandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    private bool IsInSafeZone(Vector3 position)
    {
        if (safeZone != null)
        {
            return safeZone.bounds.Contains(position);
        }
        return false;
    }
}
