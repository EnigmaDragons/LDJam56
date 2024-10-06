using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MinimapUiRenderer : MonoBehaviour
{
    [SerializeField] private RectTransform minimapRect;
    [SerializeField] private Image imagePrefab;
    [SerializeField] private Image mediumImagePrefab;
    [SerializeField] private Sprite enemySprite;
    [SerializeField] private Sprite waypointSprite;
    [SerializeField] private Sprite objectiveSprite;
    [SerializeField] private float scaleFactor = 1f;
    [SerializeField] private float waypointFadeStartDistance = 100f;
    [SerializeField] private float waypointFadeEndDistance = 200f;
    [SerializeField] private float waypointMinAlpha = 0.25f;
    
    private float minimapRadius;
   
    private Dictionary<Transform, Image> activeEnemyIcons = new Dictionary<Transform, Image>();
    private Dictionary<Transform, Image> activeWaypointIcons = new Dictionary<Transform, Image>();
    private Dictionary<Transform, Image> activeObjectiveIcons = new Dictionary<Transform, Image>();

    private void Awake()
    {
        CurrentMiniMapState.Initialize();
    }

    private void Start()
    {
        minimapRect = GetComponent<RectTransform>();
        minimapRadius = minimapRect.rect.width / minimapRect.localScale.x * 0.5f;
    }

    private Image CreateImage(Sprite sprite, bool isMedium = false)
    {
        Image newObject = Instantiate(isMedium ? mediumImagePrefab : imagePrefab, minimapRect);
        newObject.sprite = sprite;
        newObject.gameObject.SetActive(false);
        return newObject;
    }

    private void LateUpdate()
    {
        if (CurrentMiniMapState.Data.playerTransform == null)
            return;
        
        UpdateEnemies();
        UpdateWaypoints();
        UpdateObjectives();
    }

    private void UpdateEnemies()
    {
        // Create a list to store enemies that need to be removed
        List<Transform> enemiesToRemove = new List<Transform>();

        // Update existing enemies and mark inactive ones for removal
        foreach (var kvp in activeEnemyIcons)
        {
            Transform enemy = kvp.Key;
            Image enemyIcon = kvp.Value;

            if (enemy == null || !enemy.gameObject.activeInHierarchy)
            {
                enemiesToRemove.Add(enemy);
                continue;
            }

            Vector2 enemyPos = GetScaledPosition(enemy.position);
            if (IsWithinRadius(enemyPos))
            {
                enemyIcon.rectTransform.anchoredPosition = enemyPos;
                enemyIcon.gameObject.SetActive(true);
            }
            else
            {
                enemyIcon.gameObject.SetActive(false);
            }
        }

        // Remove inactive enemies
        foreach (var enemy in enemiesToRemove)
        {
            Image iconToRemove = activeEnemyIcons[enemy];
            Destroy(iconToRemove.gameObject);
            activeEnemyIcons.Remove(enemy);
        }

        foreach (var enemy in CurrentMiniMapState.Data.enemyTransforms)
        {
            if (!activeEnemyIcons.ContainsKey(enemy) && enemy != null && enemy.gameObject.activeInHierarchy)
            {
                Vector2 enemyPos = GetScaledPosition(enemy.position);               
                Image enemyIcon = CreateImage(enemySprite);
                enemyIcon.rectTransform.anchoredPosition = enemyPos;
                enemyIcon.gameObject.SetActive(true);
                activeEnemyIcons.Add(enemy, enemyIcon);
            }
        }
    }
    private void UpdateWaypoints()
    {
        List<Transform> waypointsToRemove = new List<Transform>();

        foreach (var kvp in activeWaypointIcons)
        {
            Transform waypoint = kvp.Key;
            Image waypointIcon = kvp.Value;

            if (!CurrentMiniMapState.Data.waypoints.Contains(waypoint))
            {
                waypointsToRemove.Add(waypoint);
                continue;
            }

            Vector2 waypointPos = GetScaledPosition(waypoint.position);
            Vector2 clampedPos = ClampToCircle(waypointPos);
            waypointIcon.rectTransform.anchoredPosition = clampedPos;
            waypointIcon.gameObject.SetActive(true);

            SetWaypointAlpha(waypointIcon, waypointPos.magnitude);
        }

        foreach (var waypoint in waypointsToRemove)
        {
            Image iconToRemove = activeWaypointIcons[waypoint];
            Destroy(iconToRemove.gameObject);
            activeWaypointIcons.Remove(waypoint);
        }

        foreach (var waypoint in CurrentMiniMapState.Data.waypoints)
        {
            if (!activeWaypointIcons.ContainsKey(waypoint))
            {
                Image waypointIcon = CreateImage(waypointSprite, true);
                Vector2 waypointPos = GetScaledPosition(waypoint.position);
                Vector2 clampedPos = ClampToCircle(waypointPos);
                waypointIcon.rectTransform.anchoredPosition = clampedPos;
                waypointIcon.gameObject.SetActive(true);

                SetWaypointAlpha(waypointIcon, waypointPos.magnitude);

                activeWaypointIcons.Add(waypoint, waypointIcon);
            }
        }
    }

    private void SetWaypointAlpha(Image waypointIcon, float distance)
    {
        float t = Mathf.InverseLerp(waypointFadeStartDistance, waypointFadeEndDistance, distance);
        float alpha = distance <= waypointFadeStartDistance ? 1f : Mathf.Lerp(1f, waypointMinAlpha, t);
        Color iconColor = waypointIcon.color;
        iconColor.a = alpha;
        waypointIcon.color = iconColor;
    }

    private void UpdateObjectives()
    {
        List<Transform> objectivesToRemove = new List<Transform>();

        foreach (var kvp in activeObjectiveIcons)
        {
            Transform objective = kvp.Key;
            Image objectiveIcon = kvp.Value;

            if (!CurrentMiniMapState.Data.playerObjectives.Contains(objective))
            {
                objectivesToRemove.Add(objective);
                continue;
            }

            Vector2 objectivePos = GetScaledPosition(objective.position);
            if (IsWithinRadius(objectivePos))
            {
                objectiveIcon.rectTransform.anchoredPosition = objectivePos;
                objectiveIcon.gameObject.SetActive(true);
            }
            else
            {
                objectiveIcon.rectTransform.anchoredPosition = ClampToCircle(objectivePos);
                objectiveIcon.gameObject.SetActive(true);
            }
        }

        foreach (var objective in objectivesToRemove)
        {
            Image iconToRemove = activeObjectiveIcons[objective];
            Destroy(iconToRemove.gameObject);
            activeObjectiveIcons.Remove(objective);
        }

        foreach (var objective in CurrentMiniMapState.Data.playerObjectives)
        {
            if (!activeObjectiveIcons.ContainsKey(objective))
            {
                Image objectiveIcon = CreateImage(objectiveSprite);
                Vector2 objectivePos = GetScaledPosition(objective.position);
                objectiveIcon.rectTransform.anchoredPosition = IsWithinRadius(objectivePos) ? objectivePos : ClampToCircle(objectivePos);
                objectiveIcon.gameObject.SetActive(true);
                activeObjectiveIcons.Add(objective, objectiveIcon);
            }
        }
    }

    private Vector2 GetScaledPosition(Vector3 worldPosition)
    {
        Vector3 playerPos = CurrentMiniMapState.Data.playerTransform.position;
        Vector3 relativePos = worldPosition - playerPos;
        return new Vector2(relativePos.x, relativePos.z) * scaleFactor;
    }

    private bool IsWithinRadius(Vector2 position)
    {
        return position.magnitude <= minimapRadius;
    }

    private Vector2 ClampToCircle(Vector2 position)
    {
        return Vector2.ClampMagnitude(position, minimapRadius);
    }
}
