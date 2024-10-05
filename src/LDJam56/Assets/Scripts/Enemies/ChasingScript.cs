using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasingScript : StateMachineBehaviour
{
    Transform target;
    NavMeshAgent agent;
    EnemyHandeler stats;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        stats = animator.GetComponent<EnemyHandeler>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = stats.Speed;
        agent.isStopped = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (!(Vector3.Distance(animator.transform.position, target.position) <= stats.Range))
        {
            animator.SetTrigger("idle");
            animator.ResetTrigger("run");
        }
        agent.SetDestination(target.position);
        agent.updateRotation = true;
        if (Vector3.Distance(animator.transform.position, target.position) <= stats.AttackRange)
        {
            RaycastHit hit;
            Physics.SphereCast(animator.transform.position, 0.2f, target.position - animator.transform.position, out hit, stats.AttackRange, ~(1 << animator.gameObject.layer));
            
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                animator.SetBool("attack", true);
                animator.ResetTrigger("run");
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
