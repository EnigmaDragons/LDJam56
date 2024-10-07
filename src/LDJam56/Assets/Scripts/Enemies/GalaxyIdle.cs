using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GalaxyIdle : StateMachineBehaviour
{
    GalaxyHandeler handeler;
    Transform player;
    NavMeshAgent agent;
    RaycastHit hit;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        handeler = animator.GetComponent<GalaxyHandeler>();
        player = handeler.Target;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.ResetPath();
        agent.isStopped = true;
        animator.SetTrigger("idle");
        animator.SetBool("attack2", false);
        animator.SetBool("attack1", false);
        animator.ResetTrigger("run");
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector3.Distance(animator.transform.position, player.position) < handeler.Range)
        {
            Physics.SphereCast(animator.transform.position, 0.2f, player.position - animator.transform.position, out hit, handeler.Range, ~(1 << animator.gameObject.layer));
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                animator.SetTrigger("run");
                animator.ResetTrigger("idle");
            }
        }
        Physics.SphereCast(animator.transform.position, 0.2f, player.position - animator.transform.position, out hit, handeler.AttackRange, ~(1 << animator.gameObject.layer));
        if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (Random.Range(1, 4) == 1)
                animator.SetBool("attack1", true);
            else            
                animator.SetTrigger("spell");
        }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
