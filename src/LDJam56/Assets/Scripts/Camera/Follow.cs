using UnityEngine;
using UnityEngine.Serialization;

public class Follow : MonoBehaviour
{
    [FormerlySerializedAs("cameraTransform")] [SerializeField] private Transform follower;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraFollowPosition: No target assigned!");
        }
        if (follower == null)
        {
            Debug.LogError("CameraFollowPosition: No camera transform assigned!");
        }
    }

    private void LateUpdate()
    {
      follower.position = target.position + offset;
    }
}
