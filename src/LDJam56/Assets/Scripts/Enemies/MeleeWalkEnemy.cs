using UnityEngine;

public class MeleeWalkEnemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float detectionRange = 14f;
    public GameObject deathVFXPrefab; // New field for death VFX prefab
    
    private Transform player;
    private Rigidbody rb;
    private bool playerDetected = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (player == null)
        {
            Debug.LogError("Player not found!");
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (!playerDetected && distanceToPlayer <= detectionRange)
            {
                playerDetected = true;
            }

            if (playerDetected)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                direction.y = 0; // Ensure no vertical movement
                Vector3 movement = direction * moveSpeed * Time.deltaTime;
                transform.position += new Vector3(movement.x, 0, movement.z);
            }
            else
            {
                // Wander randomly when player is not yet detected
                Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
                Vector3 wanderMovement = randomDirection * moveSpeed * 0.5f * Time.deltaTime;
                transform.position += wanderMovement;

                // Optionally, add a small chance to change direction
                if (Random.value < 0.02f) // 2% chance per frame to change direction
                {
                    transform.rotation = Quaternion.LookRotation(randomDirection);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HandlePlayerContact(other.gameObject, "Triggered");
        HandleDamagingContact(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandlePlayerContact(collision.gameObject, "Collided");
        HandleDamagingContact(collision.gameObject);
    }

    private void HandlePlayerContact(GameObject contactObject, string contactType)
    {
        if (contactObject.CompareTag("Player"))
        {
            Debug.Log($"{contactType} with player!");
            this.ExecuteAfterDelay(0.5f, () => Message.Publish(new PlayerIsDead()));
            // Add damage or other effects here
        }
    }

    private void HandleDamagingContact(GameObject contactObject)
    {
        if (contactObject.CompareTag("Damaging"))
        {
            if (deathVFXPrefab != null)
            {
                Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            }
            Message.Publish(new PlayOneShotSoundEffect(SoundEffectEnum.BotExplode, gameObject));
            gameObject.SetActive(false);
            contactObject.SetActive(false);
        }
    }
}
