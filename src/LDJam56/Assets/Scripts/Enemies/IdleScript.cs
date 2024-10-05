using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Unity.Cinemachine.CinemachineTargetGroup;
using static UnityEngine.GraphicsBuffer;

public class IdleScript : StateMachineBehaviour
{
    public EnemyHandeler stats;
    Transform player;
    NavMeshAgent agent;
    RaycastHit hit;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stats = animator.GetComponent<EnemyHandeler>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.ResetPath();
        agent.isStopped = true;
        animator.SetTrigger("idle");
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector2.Distance(animator.transform.position, player.position) <= stats.Range)
        {
            
            Physics.SphereCast(animator.transform.position, 0.2f, player.position - animator.transform.position, out hit, stats.Range, ~(1 << animator.gameObject.layer));
            if (hit.collider != null &&  hit.collider.CompareTag("Player"))
            {
                animator.SetTrigger("run");
                animator.ResetTrigger("idle");
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
