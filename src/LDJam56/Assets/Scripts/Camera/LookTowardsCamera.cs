using UnityEngine;

public class LookTowardsCamera : MonoBehaviour
{
    private Camera mainCamera;
    private bool _isInitialized = false;

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogWarning("Main camera not found. LookTowardsCamera script may not function correctly.");
        } else {
            _isInitialized = true;
        }
    }

    private void LateUpdate()
    {
        if (!_isInitialized) return;

        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                          mainCamera.transform.rotation * Vector3.up);
    }
}
