using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{
    // Combat handler script reference.
    private CombatHandler combatHandler;

    // Navmesh agent reference.
    public NavMeshAgent aiAgent;
    
    // Faceless AI pathing location transforms.
    public Transform startPos;
    public Transform turnPos;
    public Transform endPos;

    // Bool variables.
    private bool turnAround;
    private bool isRunning;

    // Distance between clone and Faceless AI.
    public float distanceBetweenClone;

    // Target positioning variables.
    private Vector3 targetPos;
    public Vector3 lastPos;
    private Vector3 clonePos;
    
    // Stops clone when on stopping crystal.
    private bool isStoppedByCrystal;
    private bool isFollowingCrystal;
    private Vector3 crytalPos;
    public float attackTimer;
    
    // Get references and initialize variables when Faceless AI is spawned.
    void Awake()
    {
        //combatHandler = FindObjectOfType<CombatHandler>();
        aiAgent = this.GetComponent<NavMeshAgent>();

        turnAround = false;
        isRunning = false;
        isStoppedByCrystal = false;
        isFollowingCrystal = false;

        distanceBetweenClone = 100f;
        attackTimer = 2f;
        
        targetPos = startPos.position;
        aiAgent.SetDestination(targetPos);
    }
    
    // Called between frames.
    void FixedUpdate()
    {
        IsCloneSpawned();
        DetermineCloneDistance();

        if (isStoppedByCrystal && isFollowingCrystal && Vector3.Distance(this.transform.position, crytalPos) < 0.1)
        {
            isStoppedByCrystal = true;
        }
    }

    // Determines whether clone is close enough to chase and attack. 
    // If not, return to pathing loop.
    void DetermineCloneDistance()
    {
        if (distanceBetweenClone < 4 && isFollowingCrystal == false)
        {
            ChaseAndAttackClone();
        }
        else if (!isStoppedByCrystal && !isFollowingCrystal)
        {
            ReturnToPath();
        }
    }

    // Sets Faceless AI to chase the clone, and attack it if they are close enough.
    void ChaseAndAttackClone()
    {
        if (distanceBetweenClone < 1.3f)
        {
            if (GameObject.FindGameObjectWithTag("Clone") != null)
            {
                if (attackTimer <= 0)
                {
                    
                    GameObject.FindGameObjectWithTag("Clone").GetComponent<ExitClone>().cloneActiveTimer -= 5f;
                    attackTimer = 2f;
                }
                else
                {
                    attackTimer -= Time.deltaTime;
                }
            }
        }
        else if (attackTimer < 2f)
        {
            attackTimer = 2f;
        }

        if (GameObject.FindWithTag("Clone") != null && !isRunning)
        {
            LookAtClone();
            FollowClone();
        }
    }

    // Return the Faceless AI to it's specified path.
    void ReturnToPath()
    {
        //combatHandler.inCombat = false;
        aiAgent.isStopped = false;
        aiAgent.SetDestination(lastPos);
    }

    // Determines whether there is a clone currently spawned in the scene.
    void IsCloneSpawned()
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
    }

    // Sets Faceless AI target position to follow the clone's position
    // and stops when close to the clone.
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
    
    // Sets Faceless AI to look at the clone.
    void LookAtClone()
    {
        var target = GameObject.FindWithTag("Clone");
            
        Quaternion lookOnLook = Quaternion.LookRotation(target.transform.position - transform.position);
 
        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * 10f);
    }

    // Sets Faceless AI target location depending on which point it just passed through.
    // Saves the last target position.
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
        else if (other.CompareTag("AIStop"))
        {
            crytalPos = other.transform.position;
            aiAgent.SetDestination(crytalPos);
            isFollowingCrystal = true;
        }
    }
}
