using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class galaxyAttacl : StateMachineBehaviour
{
    GalaxyHandeler handeler;
    Transform target;
    NavMeshAgent agent;
    Transform point1;
    Transform point2;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        handeler = animator.GetComponent<GalaxyHandeler>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.ResetPath();
        agent.isStopped = true;
        agent.speed = 2f;
        handeler.shoot = true;
        point1 = handeler.point1;
        point2 = handeler.point2;
        handeler.shoot = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Random.Range(1, 3) == 1)
            agent.SetDestination(point1.position);
        else
            agent.SetDestination(point2.position);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("attack1", false);
    }
}
