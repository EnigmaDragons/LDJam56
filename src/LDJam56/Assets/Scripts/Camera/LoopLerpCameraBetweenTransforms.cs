using UnityEngine;

public class LoopLerpCameraBetweenTransforms : MonoBehaviour
{
    [SerializeField] private Transform[] cameraPositions;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxRotationSpeed = 45f; // Maximum rotation speed in degrees per second
    [SerializeField] private float arrivalThreshold = 0.01f;

    private int currentIndex = 0;
    private Camera mainCamera;
    private float journeyLength;
    private float startTime;
    private Transform startTransform;
    private Transform endTransform;

    private void Awake()
    {
        mainCamera = Camera.main;
        if (cameraPositions.Length > 0)
        {
            SetCameraToFirstPosition();
        }
        else
        {
            Debug.LogWarning("No camera positions assigned to LoopLerpCameraBetweenTransforms.");
        }
    }

    private void Start()
    {
        if (cameraPositions.Length > 0)
        {
            InitializeNextMove();
        }
    }

    private void Update()
    {
        if (cameraPositions.Length > 0)
        {
            MoveBetweenPositions();
        }
    }

    private void SetCameraToFirstPosition()
    {
        mainCamera.transform.position = cameraPositions[0].position;
        mainCamera.transform.rotation = cameraPositions[0].rotation;
    }

    private void InitializeNextMove()
    {
        startTransform = cameraPositions[currentIndex];
        endTransform = cameraPositions[(currentIndex + 1) % cameraPositions.Length];
        journeyLength = Vector3.Distance(startTransform.position, endTransform.position);
        startTime = Time.time;
    }

    private void MoveBetweenPositions()
    {
        float distanceCovered = (Time.time - startTime) * moveSpeed;
        float fractionOfJourney = distanceCovered / journeyLength;

        // Smooth position movement
        mainCamera.transform.position = Vector3.Lerp(startTransform.position, endTransform.position, fractionOfJourney);

        // Smooth rotation with capped speed
        Quaternion targetRotation = Quaternion.Slerp(startTransform.rotation, endTransform.rotation, fractionOfJourney);
        float step = maxRotationSpeed * Time.deltaTime;
        mainCamera.transform.rotation = Quaternion.RotateTowards(mainCamera.transform.rotation, targetRotation, step);

        if (Vector3.Distance(mainCamera.transform.position, endTransform.position) <= arrivalThreshold &&
            Quaternion.Angle(mainCamera.transform.rotation, endTransform.rotation) <= arrivalThreshold)
        {
            // Ensure the camera is exactly at the end position
            mainCamera.transform.position = endTransform.position;
            mainCamera.transform.rotation = endTransform.rotation;

            currentIndex = (currentIndex + 1) % cameraPositions.Length;
            InitializeNextMove();
        }
    }
}
