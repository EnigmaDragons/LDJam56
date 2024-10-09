using UnityEngine;
using System.Collections;
using KinematicCharacterController;
using Unity.Cinemachine;

public class SpawnPlayerOnRespawn : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerTargeting;
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private CinemachineCamera cameraman;


    private void Awake()
    {
        player.SetActive(false);
        cameraman.Follow = playerTargeting.transform;
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
                Vector3 respawnPosition = respawnPoint.transform.position + new Vector3(0, 4, 0);
                TeleportUtility.TeleportPlayer(player, respawnPosition);
                Debug.Log("Player teleported to respawn point.");
            }
            else
            {
                Debug.LogWarning("No object with 'Respawn' tag found. Player spawned at origin.");
            }

            player.SetActive(true);
            cameraman.Follow = playerTargeting.transform;

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
