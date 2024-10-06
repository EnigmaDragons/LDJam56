using System;
using UnityEngine;

public static class CurrentGameState
{
    [SerializeField] private static GameState gameState = new GameState();

    public static GameState ReadonlyGameState => gameState;
    public static void Init() => gameState = new GameState();
    public static void Init(GameState initialState) => gameState = initialState;
    public static void Subscribe(Action<GameStateChanged> onChange, object owner) => Message.Subscribe(onChange, owner);
    public static void Unsubscribe(object owner) => Message.Unsubscribe(owner);
    
    public static void UpdateState(Action<GameState> apply)
    {
        UpdateState(_ =>
        {
            apply(gameState);
            return gameState;
        });
    }
    
    public static void UpdateState(Func<GameState, GameState> apply)
    {
        gameState = apply(gameState);
        Message.Publish(new GameStateChanged(gameState));
    }

    public static void DamagePlayer(bool unpreventable)
    {
        if (!unpreventable && ReadonlyGameState.PlayerStats.IsInvincible.AnyNonAlloc())
            return;
        UpdateState(s => s.PlayerStats.CurrentLife = Math.Max(0, s.PlayerStats.CurrentLife - 1));
        Message.Publish(new PlayerDamaged());
    }
}
