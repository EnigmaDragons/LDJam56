using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossHandeler : EnemyHandeler
{
    public float SpawningDelay;
    public float MeleeDelay;
    bool flag = true;
    private BossSpawner spawner;

    // Boss-specific method for spawning
    public void Spawning()
    {
        if (spawner != null)
        {
            spawner.SpawnEnemies();
        }
    }
    protected override void OnUpdate()
    {
        if(HP <= 1000f && flag)
        {
            spawner.towerSpawn();
            flag = false;
        }
        base.OnUpdate();
    }

    // Boss-specific method to set the spawner
    public void SetSpawner(BossSpawner bossSpawner)
    {
        spawner = bossSpawner;
    }

    // Override the Damaged method if you want to add boss-specific behavior when taking damage
    public override void Damaged(int damage)
    {
        base.Damaged(damage);
        // Add any boss-specific damage behavior here
    }
}
