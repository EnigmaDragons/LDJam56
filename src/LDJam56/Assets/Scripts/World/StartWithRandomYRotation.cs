using UnityEngine;

public class StartWithRandomYRotation : MonoBehaviour
{
    private void Awake()
    {
        // Generate a random rotation around the Y-axis
        float randomYRotation = Random.Range(0f, 360f);
        
        // Create a new rotation with the random Y value
        Quaternion newRotation = Quaternion.Euler(0f, randomYRotation, 0f);
        
        // Apply the new rotation to the object
        transform.rotation = newRotation;
    }
}
