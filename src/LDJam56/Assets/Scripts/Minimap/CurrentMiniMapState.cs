using System.Collections.Generic;
using UnityEngine;

public class MinimapData
{
  public List<Transform> enemyTransforms = new List<Transform>(512);
  public List<Transform> waypoints = new List<Transform>(64);
  public List<Transform> playerObjectives = new List<Transform>(32);
  public Transform playerTransform;
}

public static class CurrentMiniMapState
{
  public static MinimapData Data = new MinimapData();

  public static void Initialize()
  {
    Data = new MinimapData();
  }

  public static void SetPlayerTransform(Transform transform)
  {
    Data.playerTransform = transform;
  }

  public static void RegisterEnemy(Transform transform)
  {
    Data.enemyTransforms.Add(transform);
  }

  public static void RegisterWaypoint(Transform transform)
  {
    Data.waypoints.Add(transform);
  }

  public static void RegisterPlayerObjective(Transform transform)
  {
    Data.playerObjectives.Add(transform);
  }
}
