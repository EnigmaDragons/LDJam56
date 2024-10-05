using System.Collections.Generic;
using KinematicCharacterController;
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
                var motor = player.GetComponent<KinematicCharacterMotor>();
                if (motor != null)
                {
                    // Disable the motor
                    KinematicCharacterSystem.UnregisterCharacterMotor(motor);
                    
                    // Teleport the player to the respawn point
                    Vector3 respawnPosition = respawnPoint.transform.position + new Vector3(0, 4, 0);
                    
                    // Set the transient position and rotation
                    motor.SetPositionAndRotation(respawnPosition, player.transform.rotation);
                    
                    // Update the transform
                    player.transform.SetPositionAndRotation(respawnPosition, player.transform.rotation);
                    
                    // Re-enable the motor
                    this.ExecuteAfterDelay(0.15f, () => 
                    {
                        KinematicCharacterSystem.RegisterCharacterMotor(motor);
                        motor.SetPositionAndRotation(respawnPosition, player.transform.rotation);
                    });
                    
                    Debug.Log("Player teleported to respawn point.");
                }
                else
                {
                    Debug.LogWarning("KinematicCharacterMotor not found on player!");
                }
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
