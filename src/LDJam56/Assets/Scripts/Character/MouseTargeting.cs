using UnityEngine;

public class MouseTargeting : MonoBehaviour
{
    [SerializeField] private GameObject player;
    
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, 1 << 11))
        {
            var position = hit.point;
            position.y = 0;
            var direction = (position - new Vector3(player.transform.position.x, 0, player.transform.position.z)).normalized;
            Message.Publish(new TargetingUpdated(direction));
        }
    }
}