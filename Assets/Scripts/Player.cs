using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask whatCanBeClickedOn;
    [SerializeField] private GameObject moveMarker;
    [SerializeField] private GameObject attackMarker;
    [SerializeField] private Animator moveMarkerAnimator;
    [SerializeField] private Animator attackMarkerAnimator;
    [SerializeField] private Champion champion;

    private Animator animator;
    private bool isAgentMoving = false;
    private float timeClicked = 0;
    private NavMeshAgent myAgent;
    private bool isAttacking = false;
    private GameObject attackTarget;
    private float attackAvailableTime = 0;
    private bool isSwinging = false;

    public delegate void AttackDelegate(GameObject gObject);
    public static event AttackDelegate AttackEvent;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        myAgent = GetComponent<NavMeshAgent>();
        AttackEvent += DealDamage;
    }
    private void Start()
    {
        animator.SetFloat("AttackSpeed", champion.attackAnimationSpeed);
    }
    private void Update()
    {
        if (isAttacking)
        {
            //check if target is not Dead
            if (attackTarget == null)
            {
                GameObject nearestEnemy = GetClosestEnemy();
                if(nearestEnemy != null)
                {
                    attackTarget = nearestEnemy;
                    StartAttacking(nearestEnemy);
                }
                else
                {
                    AgentStop();
                }
            }
            else
            {
                //if target in attack range = attack, else = move to target
                LookAtTarget();
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
        if (Input.GetMouseButtonDown(1))
        {
            Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(myRay, out hitInfo, 100, whatCanBeClickedOn))
            {
                if (hitInfo.collider.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    moveMarkerAnimator.SetTrigger("DeleteMarker");
                    moveMarkerAnimator.SetTrigger("Animate");
                    Move(hitInfo.point);
                }
                else if (hitInfo.collider.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    StartAttacking(hitInfo.collider.gameObject);
                }

            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            isAttacking = false;
            animator.SetBool("isAgentAttacking", true);
            AgentStop();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartAttacking(GetClosestEnemy());
        }
        //delete moveMarker if agent arrived
        if (Vector3.Distance(moveMarker.transform.position, transform.position) < 0.2f)
        {
            moveMarkerAnimator.SetTrigger("DeleteMarker");
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
        isSwinging = false;
        isAgentMoving = false;
        animator.SetBool("isAgentAttacking", false);
        animator.SetBool("isAgentMoving", false);
        myAgent.ResetPath();
    }

    //Initiate attacking
    private void StartAttacking(GameObject collider)
    {
        attackMarker.transform.position = collider.transform.position;
        attackMarkerAnimator.SetTrigger("Animate");
        isAttacking = true;
        attackTarget = collider;
        var distanceToEnemy = attackTarget.transform.position - transform.position;
        distanceToEnemy.y = 0;
        if (distanceToEnemy.magnitude < 1)
        {
            AgentStop();
            if (attackAvailableTime <= Time.time || attackAvailableTime == 0)
            {
                Attack(attackTarget);
            }
        }
        else
        {
            animator.SetBool("isAgentAttacking", false);
            myAgent.destination = collider.transform.position;
            isAgentMoving = true;
            animator.SetBool("isAgentMoving", true);
        }
    }

    //Move player to destination
    private void Move(Vector3 destination)
    {
        isAttacking = false;
        animator.SetBool("isAgentAttacking", false);
        timeClicked = Time.time;
        moveMarker.transform.position = destination;
        myAgent.SetDestination(destination);
        isAgentMoving = true;
        animator.SetBool("isAgentMoving", true);
    }

    //Perform attack 
    private void Attack(GameObject target)
    {
        Debug.Log("prettack");
        animator.SetBool("isAgentMoving", false);
        animator.SetBool("isAgentAttacking", true);
        if(!isSwinging)
            StartCoroutine(CoSwing(target));
    }

    //Face towards attackTarget object
    private void LookAtTarget()
    {
        Vector3 lookDirection = attackTarget.transform.position - transform.position;

        Quaternion LookAtRotation = Quaternion.LookRotation(lookDirection);

        Quaternion LookAtRotationOnly_Y = Quaternion.Euler(transform.rotation.eulerAngles.x, LookAtRotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        transform.rotation = LookAtRotationOnly_Y;
    }

    //finds the closest object with Enemy script attached
    GameObject GetClosestEnemy()
    {
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Enemy e in enemies)
        {
            float dist = Vector3.Distance(e.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = e.gameObject;
                minDist = dist;
            }
        }
        return tMin;
    }

    private void DealDamage(GameObject target)
    {
        if(target != null)
            target.GetComponent<HealthAndStatus>().health -= champion.attackDamage;
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
            Debug.Log("Attack");
            isSwinging = false;
        }
    }
}
