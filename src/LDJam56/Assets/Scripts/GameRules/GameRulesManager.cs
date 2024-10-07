using UnityEngine;

public class GameRulesManager : MonoBehaviour
{
    private void OnEnable()
    {
        Message.Subscribe<PlayerIsDead>(OnPlayerIsDead, this);
        Message.Subscribe<PlayerReachedEndOfDungeon>(OnPlayerReachedEndOfDungeon, this);
    }

    private void OnDisable()
    {
        Message.Unsubscribe(this);
    }

    private void OnPlayerIsDead(PlayerIsDead message)
    {
        Debug.Log("Player is dead!");
        Message.Publish(new GameOver());
    }

    private void OnPlayerReachedEndOfDungeon(PlayerReachedEndOfDungeon message)
    {
        Debug.Log("Player reached the end of the dungeon!");
        Navigator.NavigateToCredits();
    }
}
