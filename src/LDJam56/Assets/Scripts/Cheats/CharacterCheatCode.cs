using UnityEngine;

public class CharacterCheatCode : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.KeypadPlus) && Input.GetKeyDown(KeyCode.B))
        {
            TeleportToBossRoom();
        }        
        if (Input.GetKey(KeyCode.KeypadPlus) && Input.GetKeyDown(KeyCode.T))
        {
            TeleportToServerCore();
        }
        if (Input.GetKey(KeyCode.H) && Input.GetKeyDown(KeyCode.P))
        {
            CurrentGameState.UpdateState(s =>
            {
                s.PlayerStats.MaxLife = 999;
                s.PlayerStats.CurrentLife = 999;
            });
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            #if !UNITY_WEBGL
            Application.Quit();
            #endif
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

    private void TeleportToServerCore()
    {        
      var player = GameObject.FindGameObjectWithTag("Player");
      var tp = GameObject.FindGameObjectWithTag("ServerCoreTeleport");
      if (player != null && tp != null)
      {
          Vector3 destination = tp.transform.position + Vector3.up * 2f; // Teleport 2 units above the teleport point
          TeleportUtility.TeleportPlayer(player, destination);
      }
      else
      {
          Debug.LogWarning("Cannot teleport: Player or ServerCoreTeleport not found.");
      }
    }
}