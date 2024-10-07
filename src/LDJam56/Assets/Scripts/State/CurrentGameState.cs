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
        if (gameState.PlayerStats.CurrentLife == 0)
            Message.Publish(new PlayerIsDead());
        else 
            Message.Publish(new PlayerDamaged());
    }

    public static void LowerCooldowns(float time)
    {
        if (gameState.Attack.CooldownRemaining <= 0 && gameState.Defense.CooldownRemaining <= 0 &&
            gameState.Special.CooldownRemaining <= 0 && gameState.Mobility.CooldownRemaining <= 0)
            return;
        UpdateState(s =>
        {
            s.Attack.CooldownRemaining = Math.Max(0, s.Attack.CooldownRemaining - time);
            s.Special.CooldownRemaining = Math.Max(0, s.Special.CooldownRemaining - time);
            s.Mobility.CooldownRemaining = Math.Max(0, s.Mobility.CooldownRemaining - time);
            s.Defense.CooldownRemaining = Math.Max(0, s.Defense.CooldownRemaining - time);
        });
    }

    public static Ability GetAbility(AbilityType type)
    {
        Ability ability = null;
        if (type == AbilityType.Attack)
            ability = gameState.Attack;
        if (type == AbilityType.Defense)
            ability = gameState.Defense;
        if (type == AbilityType.Mobility)
            ability = gameState.Mobility;
        if (type == AbilityType.Special)
            ability = gameState.Special;
        if (type == AbilityType.Passive)
            ability = gameState.Passives;
        return ability;
    }
}
