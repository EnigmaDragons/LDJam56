using UnityEngine;

public class MinimapPlayerRotationSync : MonoBehaviour
{
    [SerializeField] private Transform minimapIndicator;
    
    private Transform playerTransform;

    private void LateUpdate()
    {
        if (playerTransform == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                playerTransform = playerObject.transform;
            }
            else
            {
                return;
            }
        }

        if (minimapIndicator != null)
        {
            // Get the player's rotation around the y-axis
            float playerRotationY = playerTransform.eulerAngles.y;

            // Set the minimap indicator's rotation around the z-axis to match the player's y-rotation
            minimapIndicator.rotation = Quaternion.Euler(0f, 0f, -playerRotationY);
        }
    }
}
