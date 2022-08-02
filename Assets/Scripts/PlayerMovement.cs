using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask whatCanBeClickedOn;
    [SerializeField] private GameObject marker;
    private Animator animator;
    private bool isAgentMoving = false;

    private NavMeshAgent myAgent;

    private void Start()
    {
        animator = GetComponent<Animator>();
        myAgent = GetComponent<NavMeshAgent>();
    }

    private void AgentStop()
    {
        marker.SetActive(false);
        isAgentMoving = false;
        animator.SetBool("isAgentMoving", false);
        myAgent.ResetPath();
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if(Physics.Raycast(myRay, out hitInfo, 100, whatCanBeClickedOn))
            {
                marker.SetActive(true);
                marker.transform.position = hitInfo.point;
                myAgent.SetDestination(hitInfo.point);
                isAgentMoving = true;
                animator.SetBool("isAgentMoving", true);
            }
        }
        if(Input.GetKeyDown(KeyCode.S))    
        {
            Debug.Log("Agent stopped");
            AgentStop();
        }
        //delete marker if agent arrived
        if(Vector3.Distance(marker.transform.position, transform.position) < 0.2f)
        {
            marker.SetActive(false);
        }
    }
}
