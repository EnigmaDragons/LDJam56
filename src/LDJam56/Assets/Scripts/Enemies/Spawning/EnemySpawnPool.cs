using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class EnemySpawnPool : ScriptableObject
{
    public EnemySpawnRule[] enemies;

    public Queue<GameObject> GetRandomPrefabs(int n)
    {
        List<GameObject> weightedPool = new List<GameObject>();
        
        // Create a weighted pool with at least 5 times n entries
        int totalEntries = Mathf.Max(5 * n, enemies.Sum(rule => rule.weight));
        
        foreach (var rule in enemies)
        {
            int entries = Mathf.RoundToInt((float)rule.weight / enemies.Sum(r => r.weight) * totalEntries);
            for (int i = 0; i < entries; i++)
            {
                weightedPool.Add(rule.prefab);
            }
        }

        // Shuffle the weighted pool
        for (int i = weightedPool.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            GameObject temp = weightedPool[i];
            weightedPool[i] = weightedPool[j];
            weightedPool[j] = temp;
        }

        // Pick n random prefabs from the shuffled pool
        return weightedPool.Take(n).ToQueue();
    }
}
