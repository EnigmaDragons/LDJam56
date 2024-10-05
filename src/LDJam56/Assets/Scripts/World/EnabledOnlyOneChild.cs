using UnityEngine;
using System.Linq;

public class EnabledOnlyOneChild : MonoBehaviour
{
    [SerializeField] private Transform[] alwaysDisabled;
    
    private void Awake()
    {
        // Disable all children in alwaysDisabled array
        foreach (Transform child in alwaysDisabled)
        {
            if (child != null)
            {
                child.gameObject.SetActive(false);
            }
        }

        // Get all children that are not in the alwaysDisabled array
        Transform[] availableChildren = transform.Cast<Transform>()
            .Where(child => !alwaysDisabled.Contains(child))
            .ToArray();

        int totalAvailableChildren = availableChildren.Length;
        if (totalAvailableChildren == 0) return;

        int enabledChildIndex = Random.Range(0, totalAvailableChildren);

        for (int i = 0; i < totalAvailableChildren; i++)
        {
            Transform child = availableChildren[i];
            child.gameObject.SetActive(i == enabledChildIndex);
        }
    }
}
