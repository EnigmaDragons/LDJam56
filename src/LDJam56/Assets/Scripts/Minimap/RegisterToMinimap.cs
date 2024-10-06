using UnityEngine;

public class RegisterToMinimap : MonoBehaviour
{
    public enum MinimapObjectType
    {
        Enemy,
        Waypoint,
        PlayerObjective,
        Hero
    }

    [SerializeField] private MinimapObjectType objectType;

    private void Start()
    {
        switch (objectType)
        {
            case MinimapObjectType.Enemy:
                CurrentMiniMapState.RegisterEnemy(transform);
                break;
            case MinimapObjectType.Waypoint:
                CurrentMiniMapState.RegisterWaypoint(transform);
                break;
            case MinimapObjectType.PlayerObjective:
                CurrentMiniMapState.RegisterPlayerObjective(transform);
                break;
            case MinimapObjectType.Hero:
                CurrentMiniMapState.SetPlayerTransform(transform);
                break;
        }
    }
}
