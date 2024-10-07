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
    public GameObject granade;
    bool canAccess = true;
    // Start is called before the first frame update
    void Start()
    {
        point1 = transform;
    }
    public void Shoot(int flag)
    {
        
        Vector3 direction;
        if (!_targetFound)
            return;

        if (flag == 1)
            direction = firePoint.position;
        else
            direction = firePoint2.position;

        Message.Publish(new PlayOneShotSoundEffect(SoundEffectEnum.EnemyShoot, gameObject));//publishing the soundeffect i want to play(need to add to SFX enum) go to fmod soundeffects script add there new stuff
        GameObject projectile = Instantiate(projectilePrefab, direction, Quaternion.identity);
        EnemyBullets script = projectile.GetComponent<EnemyBullets>();
        script.speed = Speed;
        script.target = shootTarget.position;
        
        shoot = false;
    }
    public IEnumerator SpellCast()
    {
        if (canAccess)
        {
            canAccess = false;
            
            Instantiate(granade, new Vector3(Target.position.x, Target.position.y + 10f, Target.position.z), Quaternion.identity);
            yield return new WaitForSeconds(2f);
            Instantiate(granade, new Vector3(Target.position.x, Target.position.y + 10f, Target.position.z), Quaternion.identity);
            yield return new WaitForSeconds(2f);
            Instantiate(granade, new Vector3(Target.position.x, Target.position.y + 10f, Target.position.z), Quaternion.identity);
            canAccess = true;
        }
    }
}
