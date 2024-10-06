using UnityEngine;

public class RandomMaterial : MonoBehaviour
{
    [SerializeField] private Material[] materials;

    private void Awake()
    {
        if (materials == null || materials.Length == 0)
        {
            Debug.LogWarning("No materials assigned to RandomMaterial script on " + gameObject.name);
            return;
        }

        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogError("No Renderer component found on " + gameObject.name);
            return;
        }

        Material randomMaterial = materials[Random.Range(0, materials.Length)];
        renderer.material = randomMaterial;
    }
}
