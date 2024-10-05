using UnityEngine;
using UnityEngine.Events;

public class KillTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        HandleEnemyContact(other.gameObject, true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleEnemyContact(collision.gameObject, true);
    }
    
    private void HandleEnemyContact(GameObject contactObject, bool isEnter)
    {
        if (!contactObject.CompareTag("Player"))
        {
            if (isEnter)
            {
                contactObject.SetActive(false);
            }

        }
    }
}
