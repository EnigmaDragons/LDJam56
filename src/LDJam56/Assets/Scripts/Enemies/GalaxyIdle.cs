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
    bool flag;
    float timer = 0f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        handeler = animator.GetComponent<GalaxyHandeler>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.ResetPath();
        agent.isStopped = true;
        animator.SetTrigger("idle");
        animator.SetBool("attack2", false);
        animator.SetBool("attack1", false);
        animator.ResetTrigger("run");
        flag = true;
        timer = 0f;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { timer += Time.deltaTime;
        if (Vector3.Distance(animator.transform.position, player.position) < handeler.Range)
        {
            
                animator.SetTrigger("run");
               animator.ResetTrigger("idle");
            
        }
        
        if(Vector3.Distance(animator.transform.position, player.position) < handeler.AttackRange && timer >= handeler.AttackDelay)
        if (flag)
        {
                timer = 0f;   
            if (Random.Range(1, 4) == 1)
                animator.SetBool("attack1", true);
            else
                animator.SetTrigger("spell");
            flag = false;
        }
        
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
