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
    private float stuckThreshold = 0.1f;
    private float stuckTime = 1.5f; 
    private float stuckTimer = 0.0f;
    private Vector3 lastPosition;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        handeler = animator.GetComponent<GalaxyHandeler>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = handeler.Speed;
        agent.isStopped = false;
        lastPosition = agent.transform.position;

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
        Vector3 directionToTarget = target.position - agent.transform.position;
        Vector3 oppositeDirection = -directionToTarget;
        Vector3 destination = agent.transform.position + oppositeDirection;
        agent.SetDestination(destination);
        agent.updateRotation = true;

        if (Vector3.Distance(agent.transform.position, lastPosition) < stuckThreshold)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer >= stuckTime)
            {
                
                Vector3 randomDirection = Random.insideUnitSphere * 5.0f;
                randomDirection += agent.transform.position;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDirection, out hit, 5.0f, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }
                stuckTimer = 0.0f;
            }
        }
        else
        {
            stuckTimer = 0.0f;
        }

        lastPosition = agent.transform.position;
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("run");
    }
}
