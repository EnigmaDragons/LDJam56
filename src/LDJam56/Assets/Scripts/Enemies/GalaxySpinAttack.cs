using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GalaxySpinAttack : StateMachineBehaviour
{
    Transform target;
    NavMeshAgent agent;
    GalaxyHandeler handeler;
    float timer=0f;
    float timer2;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        handeler = animator.GetComponent<GalaxyHandeler>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = handeler.Speed *1.5f;
        agent.isStopped = false;
        agent.updateRotation = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        
        if (timer >= handeler.spinDuration)
        {
            animator.SetBool("attack2", false);
            timer = 0f;
        }
        
        agent.SetDestination(target.position);

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
