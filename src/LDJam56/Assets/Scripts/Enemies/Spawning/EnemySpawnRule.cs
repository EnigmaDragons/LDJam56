using UnityEngine;

[CreateAssetMenu]
public class EnemySpawnRule : ScriptableObject
{
    public EnemyHandlerHolder prefab;
    public int weight = 1;
}
