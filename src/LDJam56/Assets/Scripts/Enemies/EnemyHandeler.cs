using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;
using static Unity.Cinemachine.CinemachineTargetGroup;

public class EnemyHandeler : MonoBehaviour
{

    public Transform Target;
    public float HP;
    public float Speed;
    public float Range;
    public float AttackRange;
    public float Attack2Range;
    public float Damage;

    public float AttackDelay;
    public float Attack2Delay;

    public Transform firePoint;
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public bool shoot = false;
    public Transform shootTarget;

    public Collider Collider1;
    public Collider Collider2;
    
    private bool _targetFound;

    NavMeshAgent agent;
    Animator animator;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!_targetFound)
        {
            var obj = GameObject.FindWithTag("Player");
            if (obj != null)
            {
                Target = obj.transform;
                _targetFound = true;
                if (shootTarget == null)
                    shootTarget = Target;
            }
        }
        
        if (HP <= 0)
        {
            animator.SetTrigger("death");
            Destroy(this.gameObject, animator.GetCurrentAnimatorClipInfo(0).Length);
        }
        if(shoot && agent.updateRotation == false)
        {
            Vector3 direction = Target.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
        }

    }
    public void BeginAttackBasic1()
    {
        Collider1.enabled = true;
    }
    public void EndAttackBasic1()
    {
        Collider1.enabled = false;
    }
    public void BeginAttackBasic2()
    {
        Collider2.enabled = true;
    }
    public void EndAttackBasic2()
    {
        Collider2.enabled = false;
    }
    public void BasicShoot()
    {
        if (!_targetFound)
            return;

        Vector3 direction = (shootTarget.position - firePoint.position).normalized;

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
        agent.updateRotation = true;
    }

    public void Damaged(int damage)
    {
        HP -= damage;
        if(HP > 0)
            animator.SetTrigger("hit");
    }
    
}
