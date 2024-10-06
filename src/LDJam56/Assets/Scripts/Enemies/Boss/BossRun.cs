using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossRun : StateMachineBehaviour
{
    Transform target;
    NavMeshAgent agent;
    BossHandeler handeler;
    float timer;
    float timer2;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0f;
        handeler = animator.GetComponent<BossHandeler>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = handeler.Speed;
        agent.isStopped = false;
        animator.ResetTrigger("attack2");
        animator.ResetTrigger("attack1");
        animator.SetTrigger("run");
        handeler.BeginAttackBasic1();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        timer2 += Time.deltaTime;
        if (timer >= handeler.MeleeDelay && Mathf.Abs(Vector3.Distance(animator.transform.position, target.position)) < handeler.AttackRange)
        {
            timer = 0f;
            if (Random.Range(1, 6) < 4)
                animator.SetTrigger("attack");
            else
                animator.SetTrigger("attack2");
        }
        agent.SetDestination(target.position);
        agent.updateRotation = true;
        if(timer2 >= handeler.MeleeDelay * 2.8)
        {
            timer2 = 0f;
            animator.ResetTrigger("run");
            animator.SetTrigger("idle");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
