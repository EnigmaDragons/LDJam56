using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasingScript : StateMachineBehaviour
{
    Transform target;
    NavMeshAgent agent;
    EnemyHandeler stats;
    float timer;
    float timer2;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        stats = animator.GetComponent<EnemyHandeler>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = stats.Speed;
        agent.isStopped = false;
        animator.SetBool("attack2", false);
        animator.SetTrigger("run");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        timer2 += Time.deltaTime;
        if (timer >= stats.Attack2Delay && Vector3.Distance(animator.transform.position, target.position) < stats.Attack2Range)
        {
            animator.SetBool("attack1", false);
            animator.SetBool("attack2", true); 
            timer2 = 0f;
        }

        if (!(Vector3.Distance(animator.transform.position, target.position) <= stats.Range))
        {
            animator.SetTrigger("idle");
            animator.ResetTrigger("run");
        }
        agent.SetDestination(target.position);
        agent.updateRotation = true;
        if (Vector3.Distance(animator.transform.position, target.position) < stats.AttackRange && timer >= stats.AttackDelay)
        {
            timer = 0f;
           
                animator.SetBool("attack", true);
                animator.ResetTrigger("run");
            
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
