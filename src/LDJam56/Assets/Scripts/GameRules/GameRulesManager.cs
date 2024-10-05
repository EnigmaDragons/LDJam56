using System.Collections.Generic;
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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Find the respawn point
            GameObject respawnPoint = GameObject.FindGameObjectWithTag("Respawn");
            if (respawnPoint != null)
            {
                // Teleport the player to the respawn point
                player.transform.position = respawnPoint.transform.position + new Vector3(0, 4, 0);
                Debug.Log("Player teleported to respawn point.");
            }
            else
            {
                Debug.LogWarning("Respawn point not found!");
            }
        }
        else
        {
            Debug.LogWarning("Player not found!");
        }
    }

    private void OnPlayerReachedEndOfDungeon(PlayerReachedEndOfDungeon message)
    {
        Debug.Log("Player reached the end of the dungeon!");
        Navigator.NavigateToCredits();
    }
}
