using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossHandeler : MonoBehaviour
{
    public Transform Target;
    public float HP;
    public float Speed;
    public float Range;
    public float AttackRange;
    public float Attack2Range;
    public float Damage;

    public float SpawningDelay;
    public float MeleeDelay;

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
        if (shoot && agent.updateRotation == false)
        {
            Vector3 direction = Target.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
        }

    }
    public void BeginAttackBasic1()
    {
        Collider1.enabled = true;
        Collider2.enabled = true;
    }
    public void EndAttackBasic1()
    {
        Collider1.enabled = false;
        Collider2.enabled = false;
    }
    public void Spawning()
    {
        //use the pinned script and some help from silas create a new script from hes and modifiy it
    }

    public void Damaged(int damage)
    {
        HP -= damage;
    }
}
