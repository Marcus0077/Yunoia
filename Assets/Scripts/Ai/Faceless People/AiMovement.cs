using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{
    private CombatHandler combatHandler;
    public NavMeshAgent aiAgent;
    
    public Transform startPos;
    public Transform turnPos;
    public Transform endPos;

    private bool turnAround;
    private bool isRunning;

    public float distanceBetweenClone;

    private Vector3 targetPos;
    public Vector3 lastPos;
    private Vector3 clonePos;
    void Awake()
    {
        combatHandler = FindObjectOfType<CombatHandler>();
        aiAgent = this.GetComponent<NavMeshAgent>();

        turnAround = false;
        isRunning = false;

        distanceBetweenClone = 100f;
        
        targetPos = startPos.position;
        aiAgent.SetDestination(targetPos);
    }
    
    void FixedUpdate()
    {
        if (GameObject.FindWithTag("Clone") != null)
        {
            clonePos = GameObject.FindWithTag("Clone").transform.position;
            distanceBetweenClone = Vector3.Distance(this.transform.position, clonePos);
        }
        else
        {
            distanceBetweenClone = 100f;
        }

        if (distanceBetweenClone < 4)
        {
            if (distanceBetweenClone < 1.3f)
            {
                combatHandler.inCombat = true;
            }
            else
            {
                combatHandler.inCombat = false;
            }

            if (GameObject.FindWithTag("Clone") != null && !isRunning)
            {
                LookAtClone();
                FollowClone();
            }
        }
        else
        {
            combatHandler.inCombat = false;
            aiAgent.isStopped = false;
            aiAgent.SetDestination(lastPos);
        }
    }

    void FollowClone()
    {
        if (distanceBetweenClone < 1.5f)
        {
            aiAgent.isStopped = true;
        }
        else
        {
            aiAgent.isStopped = false;
            
            targetPos = clonePos;
            aiAgent.SetDestination(targetPos);
        }
    }
    
    void LookAtClone()
    {
        var target = GameObject.FindWithTag("Clone");
            
        Quaternion lookOnLook = Quaternion.LookRotation(target.transform.position - transform.position);
 
        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StartPoint"))
        {
            turnAround = false;

            lastPos = turnPos.position;
            aiAgent.SetDestination(lastPos);
        }
        else if (other.CompareTag("TurnPoint"))
        {
            if (turnAround)
            {
                lastPos = startPos.position;
                aiAgent.SetDestination(lastPos);
            }
            else
            {
                lastPos = endPos.position;
                aiAgent.SetDestination(lastPos);
            }
        }
        else if (other.CompareTag("EndPoint"))
        {
            turnAround = true;

            lastPos = turnPos.position;
            aiAgent.SetDestination(lastPos);
        }
    }
}
