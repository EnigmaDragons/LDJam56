using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

public class ServerCoreCameraThing : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cameraman;
    [SerializeField] private CinemachinePositionComposer positionComposer;
    [SerializeField] private float finalPositionDistance = 84;
    [SerializeField] private float lerpDuration = 5f;
    private CinemachineTargetGroup targetGroup;

    private float _initialPositionDistance;
    
    public void DoIt()
    {
        GroupVersion();
    }
    
    private void PositionFollowVersion()
    {
        StartCoroutine(LerpCameraDistance());
    }
    
    private void GroupVersion()
    {
        // Create a new GameObject for the target group
        GameObject targetGroupObject = new GameObject("DynamicTargetGroup");
        targetGroup = targetGroupObject.AddComponent<CinemachineTargetGroup>();

        // Find objects with tags
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject serverCore = GameObject.FindGameObjectWithTag("ServerCore");

        // Add targets to the group
        if (player != null && serverCore != null)
        {
            targetGroup.AddMember(player.transform, 1, 2);
            targetGroup.AddMember(serverCore.transform, 1, 2);

            // Set the virtual camera to follow and look at the target group
            if (cameraman != null)
            {
                _initialPositionDistance = positionComposer.CameraDistance;
                StartCoroutine(LerpCameraDistance());
                cameraman.Follow = targetGroup.transform;
                cameraman.LookAt = targetGroup.transform;
            }
            else
            {
                Debug.LogError("Virtual Camera reference is missing!");
            }
        }
        else
        {
            Debug.LogError("Could not find Player or ServerCore!");
        }
    }

    private IEnumerator LerpCameraDistance()
    {
        float elapsedTime = 0f;
        while (elapsedTime < lerpDuration)
        {
            float t = elapsedTime / lerpDuration;
            positionComposer.CameraDistance = Mathf.Lerp(_initialPositionDistance, finalPositionDistance, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        positionComposer.CameraDistance = finalPositionDistance;
    }
}    
