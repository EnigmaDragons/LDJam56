using UnityEngine;

public class BasicShoot : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public Transform firePoint;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 targetPoint = ray.GetPoint(rayDistance);
            targetPoint.y = firePoint.position.y; // Set Y to player's height

            Vector3 direction = (targetPoint - firePoint.position).normalized;

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Message.Publish(new PlayOneShotSoundEffect(SoundEffectEnum.ShootOne, projectile));
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

            if (projectileRb != null)
            {
                projectileRb.velocity = direction * projectileSpeed;
            }
            else
            {
                Debug.LogWarning("Projectile prefab is missing a Rigidbody component!");
            }
        }
    }
}
