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
    public float Damage;

    public float AttackDelay;

    public Transform firePoint;
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public bool shoot = false;

    public Collider Collider1;
    public Collider Collider2;

    NavMeshAgent agent;
    Animator animator;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        animator = GetComponent<Animator>();
        Target = GameObject.FindWithTag("Player").transform;
    }
    private void Update()
    {
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
        Transform target = GameObject.FindWithTag("Player").transform;
        Vector3 direction = (target.position - firePoint.position).normalized;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Damaging"))
        {
            HP--;//later we can change to custom
            if(HP > 0)
            animator.SetTrigger("hit");
            
        }
    }

}
