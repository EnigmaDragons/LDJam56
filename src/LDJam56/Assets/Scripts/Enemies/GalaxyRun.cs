using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GalaxyRun : StateMachineBehaviour
{
    Transform target;
    NavMeshAgent agent;
    GalaxyHandeler handeler;
    float timer;
    float timer2;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        handeler = animator.GetComponent<GalaxyHandeler>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = handeler.Speed;
        agent.isStopped = false;
        

        //reset all interactions
        animator.SetBool("attack2", false);
        animator.SetBool("attack1", false);
        animator.ResetTrigger("spell");
        animator.ResetTrigger("idle");

        animator.SetTrigger("run");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        timer2 += Time.deltaTime;
        if (timer >= handeler.Attack2Delay && Vector3.Distance(animator.transform.position, target.position) < handeler.Attack2Range)
        {
            animator.SetBool("attack1", false);
            animator.SetBool("attack2", true);
            timer = 0f;
        }

        if ((Vector3.Distance(animator.transform.position, target.position) >= handeler.AttackRange))
        {
            animator.SetTrigger("idle");
            animator.ResetTrigger("run");
        }
        agent.SetDestination(new Vector3(-target.position.x,target.position.y,-target.position.z));
        agent.updateRotation = true;
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("run");
    }
}
