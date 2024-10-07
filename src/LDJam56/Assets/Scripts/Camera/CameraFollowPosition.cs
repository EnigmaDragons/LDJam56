using UnityEngine;

public class CameraFollowPosition : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraFollowPosition: No target assigned!");
            return;
        }
        if (cameraTransform == null)
        {
            Debug.LogError("CameraFollowPosition: No camera transform assigned!");
            return;
        }
    }

    private void LateUpdate()
    {
      cameraTransform.position = target.position + offset;
    }
}
