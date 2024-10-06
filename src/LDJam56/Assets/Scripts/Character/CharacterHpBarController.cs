using System;
using UnityEngine;
using RengeGames.HealthBars;

public class CharacterHpBarController : OnMessage<GameStateChanged>
{
    [SerializeField] private RadialSegmentedHealthBar healthBar;

    private void Start()
    {
        if (healthBar == null)
        {
            healthBar = GetComponent<RadialSegmentedHealthBar>();
        }

        if (healthBar == null)
        {
            Debug.LogError("RadialSegmentedHealthBar component not found!");
        }
        else
        {
            SyncHealthBar(CurrentGameState.ReadonlyGameState);
        }
    }

    protected override void Execute(GameStateChanged msg)
    {
        SyncHealthBar(msg.State);
    }

    private void SyncHealthBar(GameState g)
    {
        if (!(Math.Abs(healthBar.SegmentCount.Value - g.PlayerStats.MaxLife) > 0.01) &&
            !(Math.Abs(healthBar.RemoveSegments.Value - (g.PlayerStats.MaxLife - g.PlayerStats.CurrentLife)) > 0.01)) return;
        
        var maxHealth = g.PlayerStats.MaxLife;
        var currentHealth = g.PlayerStats.CurrentLife;

        healthBar.SetSegmentCount(maxHealth);
        StartCoroutine(healthBar.AnimateChange(maxHealth, currentHealth, 1.5f));
    }
}
