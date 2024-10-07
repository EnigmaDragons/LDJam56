using Unity.Cinemachine;
using UnityEngine;

public class ServerCoreCameraThing : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineTargetGroup targetGroup;

    void Start()
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
            if (virtualCamera != null)
            {
                virtualCamera.Follow = targetGroup.transform;
                virtualCamera.LookAt = targetGroup.transform;
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
}    
