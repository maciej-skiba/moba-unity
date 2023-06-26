using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeMinion : MonoBehaviour
{
    private Champion champion;
    private Animator animator;
    private bool isAgentMoving = false;
    private float timeClicked = 0;
    private NavMeshAgent myAgent;
    private bool isAttacking = false;
    private GameObject attackTarget;
    private float attackAvailableTime = 0;
    private bool isSwinging = false;
    private DetectPlayer playerDetecter;
    private float meleeMinionDamage = 10.0f;
    private HealthAndStatus healthAndStatus;

    public delegate void AttackDelegate(GameObject gObject);
    public static event AttackDelegate AttackEvent;

    private void Awake()
    {
        champion = FindObjectOfType<Champion>();
        attackTarget = champion.gameObject;
        animator = GetComponent<Animator>();
        myAgent = GetComponent<NavMeshAgent>();
        AttackEvent += DealDamage;
        playerDetecter = GetComponent<DetectPlayer>();
        healthAndStatus = GetComponent<HealthAndStatus>();
    }
    private void Start()
    {
        animator.SetFloat("AttackSpeed", champion.attackAnimationSpeed);
    }
    private void Update()
    {
        if(healthAndStatus.isStunned)
        {
            AgentStop();
            return;
        }
        if (playerDetecter.followingPlayer)
        {
            //check if target is not Dead
            if (champion == null)
            {
                AgentStop();
                playerDetecter.followingPlayer = false;
            }
            else
            {
                //if target in attack range = attack, else = move to target
                Movement.LookAtTarget(gameObject, attackTarget);
                var distanceToAttacker = Vector3.Distance(transform.position, attackTarget.transform.position);
                if (distanceToAttacker < champion.attackRange)
                {
                    myAgent.ResetPath();
                    isAgentMoving = false;
                    if (attackAvailableTime <= Time.time)
                    {
                        Attack(attackTarget);
                    }
                }
                else
                {
                    //Debug.Log("Distance to attacker = " + Vector3.Distance(transform.position, attackTarget.transform.position));
                    if (!isAgentMoving)
                    {
                        animator.SetBool("isAgentAttacking", false);
                        timeClicked = Time.time;
                        isAgentMoving = true;
                        animator.SetBool("isAgentMoving", true);
                        myAgent.SetDestination(attackTarget.transform.position);
                    }
                }
            }
        }
        else
        {
            AgentStop();
        }

        //if agent has reached its destination - stay
        if (timeClicked + 0.2f < Time.time)
        {
            if (!myAgent.hasPath && animator.GetBool("isAgentMoving"))
            {
                AgentStop();
            }
        }
    }
    private void AgentStop()
    {
        isAttacking = false;
        isSwinging = false;
        isAgentMoving = false;
        animator.SetBool("isAgentAttacking", false);
        animator.SetBool("isAgentMoving", false);
        myAgent.ResetPath();
    }

    //Perform attack 
    private void Attack(GameObject target)
    {
        isAttacking = true;
        animator.SetBool("isAgentMoving", false);
        animator.SetBool("isAgentAttacking", true);
        if (!isSwinging)
            StartCoroutine(CoSwing(target));
    }

    private void DealDamage(GameObject target)
    {
        if (target != null)
            target.GetComponent<HealthAndStatus>().health -= meleeMinionDamage;
    }

    //attack in the swing moment, not in the start of attack animation
    IEnumerator CoSwing(GameObject target)
    {
        isSwinging = true;
        float swingTime = (champion.hitTime / champion.attackAnimationSpeed);
        yield return new WaitForSeconds(swingTime);
        if (isAttacking)
        {
            AttackEvent(target);
            attackAvailableTime = Time.time + champion.attackSpeed - swingTime;
            isSwinging = false;
        }
    }
}
