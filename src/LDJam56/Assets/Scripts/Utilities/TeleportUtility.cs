using System;
using UnityEngine;
using KinematicCharacterController;

public static class TeleportUtility
{
    public static void TeleportPlayer(GameObject player, Vector3 destination)
    {
        var motor = player.GetComponent<KinematicCharacterMotor>();
        if (motor != null)
        {
            // Disable the motor
            KinematicCharacterSystem.UnregisterCharacterMotor(motor);
            
            // Set the transient position and rotation
            motor.SetPositionAndRotation(destination, player.transform.rotation);
            
            // Update the transform
            player.transform.SetPositionAndRotation(destination, player.transform.rotation);

            Action reset = () =>
            {
                KinematicCharacterSystem.RegisterCharacterMotor(motor);
                motor.SetPositionAndRotation(destination, player.transform.rotation);
            };
            
            // Re-enable the motor
            MonoBehaviour monoBehaviour = player.GetComponent<MonoBehaviour>();
            if (monoBehaviour != null)
            {
                monoBehaviour.ExecuteAfterDelay(0.15f, reset);
            }
            else
            {
                Debug.LogWarning("No MonoBehaviour found on player. Immediate registration will be performed.");
                reset();
            }
            
            Debug.Log("Player teleported to destination.");
        }
        else
        {
            Debug.LogWarning("KinematicCharacterMotor not found on player. Teleportation may not work as expected.");
            player.transform.position = destination;
        }
    }
}
