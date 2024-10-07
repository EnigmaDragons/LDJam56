using UnityEngine;

[CreateAssetMenu]
public class EventPublisher : ScriptableObject
{
    public static void PublishPlayerReachedEndOfDungeon() => Message.Publish(new PlayerReachedEndOfDungeon());
    public static void PublishPlayerIsDead() => Message.Publish(new PlayerIsDead());
    public static void PublishServerCoreRoomEntered() => Message.Publish(new ServerCoreRoomEntered());
}
