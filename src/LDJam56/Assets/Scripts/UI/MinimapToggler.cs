using UnityEngine;

public class MinimapCamControl : MonoBehaviour
{
    [SerializeField] private GameObject[] normalModeObjects;
    [SerializeField] private GameObject[] tabModeObjects;

    private bool isTabPressed = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isTabPressed = true;
            ToggleTabMode(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            isTabPressed = false;
            ToggleTabMode(false);
        }
    }

    private void Start()
    {
        SetObjectsActive(normalModeObjects, true);
        SetObjectsActive(tabModeObjects, false);
    }

    private void ToggleTabMode(bool isTabPressed)
    {
        SetObjectsActive(normalModeObjects, !isTabPressed);
        SetObjectsActive(tabModeObjects, isTabPressed);
    }

    private void SetObjectsActive(GameObject[] objects, bool isActive)
    {
        foreach (var obj in objects)
        {
            if (obj != null)
            {
                obj.SetActive(isActive);
            }
        }
    }
}
