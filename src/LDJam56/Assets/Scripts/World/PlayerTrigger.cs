using UnityEngine;
using UnityEngine.Events;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent onEnter;
    [SerializeField] private UnityEvent onExit;
    
    private BoxCollider triggerCollider;
    private bool playerInside = false;

    private void Start()
    {
        triggerCollider = GetComponent<BoxCollider>();
        if (triggerCollider == null)
        {
            Debug.LogError("BoxCollider component not found on PlayerTrigger object.", this);
        }
    }

    private void Update()
    {
        if (triggerCollider != null)
        {
            Collider[] colliders = Physics.OverlapBox(triggerCollider.bounds.center, triggerCollider.bounds.extents, transform.rotation, LayerMask.GetMask("Player"));
            bool playerDetected = colliders.Length > 0;

            if (playerDetected && !playerInside)
            {
                HandlePlayerInteraction(colliders[0].gameObject, true);
                playerInside = true;
            }
            else if (!playerDetected && playerInside)
            {
                HandlePlayerInteraction(null, false);
                playerInside = false;
            }
        }
    }

    private void HandlePlayerInteraction(GameObject obj, bool isEnter)
    {
        if (isEnter)
        {
            Debug.Log($"Player entered", this);
            onEnter.Invoke();
        }
        else
        {
            Debug.Log($"Player exited", this);
            onExit.Invoke();
        }
    }
}
