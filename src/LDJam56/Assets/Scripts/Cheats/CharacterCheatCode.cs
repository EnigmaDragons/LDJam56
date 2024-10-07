using UnityEngine;

public class CharacterCheatCode : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.KeypadPlus) && Input.GetKeyDown(KeyCode.B))
        {
            TeleportToBossRoom();
        }
    }

    private void TeleportToBossRoom()
    {        
      var player = GameObject.FindGameObjectWithTag("Player");
      var bossRoomTeleport = GameObject.FindGameObjectWithTag("BossRoomTeleport");
      if (player != null && bossRoomTeleport != null)
      {
          Vector3 destination = bossRoomTeleport.transform.position + Vector3.up * 2f; // Teleport 2 units above the teleport point
          TeleportUtility.TeleportPlayer(player, destination);
      }
      else
      {
          Debug.LogWarning("Cannot teleport: Player or BossRoomTeleport not found.");
      }
    }
}