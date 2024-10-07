using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyHandeler : EnemyHandeler
{
    public Transform point1;
    public Transform point2;
    public Transform firePoint2;
    public float spinDuration;
    // Start is called before the first frame update
    void Start()
    {
        point1 = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Shoot(int flag)
    {
        Vector3 direction;
        if (!_targetFound)
            return;

        if(flag == 1)
            direction = (shootTarget.position - firePoint.position).normalized;
        else
            direction = (shootTarget.position - firePoint2.position).normalized;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Message.Publish(new PlayOneShotSoundEffect(SoundEffectEnum.EnemyShoot, gameObject));//publishing the soundeffect i want to play(need to add to SFX enum) go to fmod soundeffects script add there new stuff
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        if (projectileRb != null)
        {
            projectileRb.velocity = direction * projectileSpeed;
        }
        else
        {
            Debug.LogWarning("Projectile prefab is missing a Rigidbody component!");
        }
        shoot = false;
    }
    public IEnumerator SpellCast()
    {
        yield return new WaitForSeconds(1f);
        //big metheor at player location
        yield return new WaitForSeconds(1f);
        //big metheor at player location
        yield return new WaitForSeconds(1f);
        //big metheor at player location
    }
}
