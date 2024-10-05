using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;

public class EnemyHandeler : MonoBehaviour
{
    public float HP;
    public float Speed;
    public float Range;
    public float AttackRange;
    public float Damage;
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
        if (HP <= 0)
        {
            animator.SetTrigger("death");
            Destroy(this.gameObject, animator.GetCurrentAnimatorClipInfo(0).Length);
        }

    }
    public void AttackBasic()
    {
        //attack
    }
    public void EndAttackBasic()
    {
        //stop it or just stop with time
    }
    public void DesTroyEnemie()
    {
        
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
