﻿using UnityEngine;

[CreateAssetMenu]
public class EventPublisher : ScriptableObject
{
    public static void PublishPlayerReachedEndOfDungeon() => Message.Publish(new PlayerReachedEndOfDungeon());
    public static void PublishPlayerIsDead() => Message.Publish(new PlayerIsDead());
    public static void PublishGameWon() => Message.Publish(new GameWon());
    public static void PublishServerCoreRoomEntered() => Message.Publish(new ServerCoreRoomEntered());

    public static void SetInfiniteRangeProjectile() => CurrentGameState.UpdateState(g =>
    {
        g.PlayerStats.Range = 999f;
    });
}
