using UnityEngine;
using System.Collections;
using KinematicCharacterController;
using Unity.Cinemachine;

public class SpawnPlayerOnRespawn : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private CinemachineCamera cameraman;


    private void Awake()
    {
        player.SetActive(false);
        cameraman.Follow = player.transform;
    }

    private void Start()
    {
        StartCoroutine(SpawnPlayerWithDelay());
    }

    private IEnumerator SpawnPlayerWithDelay()
    {
        yield return new WaitForSeconds(0.1f);

        if (player != null)
        {
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
            }
            else
            {
                Debug.LogWarning("No object with 'Respawn' tag found. Player spawned at origin.");
            }

            player.SetActive(true);
            cameraman.Follow = player.transform;

            if (particlePrefab != null)
            {
                Instantiate(particlePrefab, player.transform.position, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogError("Player object is null. Unable to spawn player.");
        }
    }
}
