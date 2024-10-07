using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Unity.Cinemachine.CinemachineTargetGroup;
using static UnityEngine.GraphicsBuffer;

public class IdleScript : StateMachineBehaviour
{
    EnemyHandeler stats;
    Transform player;
    NavMeshAgent agent;
    RaycastHit hit;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stats = animator.GetComponent<EnemyHandeler>();        
        var pObj = GameObject.FindGameObjectWithTag("Player");
        if (pObj != null)
            player = pObj.transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.ResetPath();
        agent.isStopped = true;
        animator.SetTrigger("idle");
        animator.SetBool("attack2", false);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null)
        {
            var pObj = GameObject.FindGameObjectWithTag("Player");
            if (pObj != null)
                player = pObj.transform;
        }
        if (player != null)
        {
            var pDist = Vector3.Distance(animator.transform.position, player.position);
            if (pDist < stats.Range)
            {
                
                    animator.SetTrigger("run");
                    animator.ResetTrigger("idle");
                
            }

            if (pDist < stats.Attack2Range)         
            { 
                animator.SetBool("attack2", true); 
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
