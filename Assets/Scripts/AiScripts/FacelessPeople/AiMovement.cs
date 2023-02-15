using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AiMovement : MonoBehaviour
{
    // Navmesh agent reference.
    [SerializeField] private NavMeshAgent aiAgent;

    // Bool variables.
    private bool isRunning;

    // Distance between clone and Faceless AI.
    private float distanceBetweenClone;

    // Target positioning variables.
    private Vector3 targetPos;
    private Vector3 lastPos;
    private Vector3 clonePos;
    
    // Stops clone when on stopping crystal.
    private bool isStoppedByCrystal;
    private bool isFollowingCrystal;
    private Vector3 crytalPos;

    // AI Wandering Variables.
    private float wanderDistance;
    
    [Range(1.0f, 10.0f)]
    public float wanderDistanceMin;
    
    [Range(1.0f, 10.0f)]
    public float wanderDistanceMax;
    
    [Range(0.5f, 5.0f)]
    public float wanderPauseMin;
    
    [Range(0.5f, 5.0f)]
    public float wanderPauseMax;
    
    // Get references and initialize variables when Faceless AI is spawned.
    private void Awake()
    {
        aiAgent = this.GetComponent<NavMeshAgent>();

        isRunning = false;
        isStoppedByCrystal = false;
        isFollowingCrystal = false;

        distanceBetweenClone = 100f;
    }
    
    // Called between frames.
    private void FixedUpdate()
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
    private void DetermineCloneDistance()
    {
        if (distanceBetweenClone < 4 && isFollowingCrystal == false)
        {
            StopCoroutine(Wander());
            ChaseClone();
        }
        else if (!isStoppedByCrystal && !isFollowingCrystal && !isRunning)
        {
            aiAgent.isStopped = false;

            StartCoroutine(Wander());
        }
    }

    // Sets Faceless AI to chase the clone, and attack it if they are close enough.
    private void ChaseClone()
    {
        if (GameObject.FindWithTag("Clone") != null && !isRunning)
        {
            LookAtClone();
            FollowClone();
        }
    }

    private Vector3 RandomNavSphere(Vector3 origin, float distance, int areaMask)
    {
        Vector3 randomDirection = new Vector3(UnityEngine.Random.insideUnitSphere.x,
            0f, UnityEngine.Random.insideUnitSphere.z) * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, areaMask);

        return navHit.position;
    }

    private IEnumerator Wander()
    {
        isRunning = true;

        bool canWalk;
        bool walked = false;
        
        wanderDistance = Random.Range(wanderDistanceMin, wanderDistanceMax);

        while (!walked)
        {
            canWalk = true;
            
            targetPos = RandomNavSphere(this.transform.position, wanderDistance, 1);

            foreach (var plantDestroyers in GameObject.FindObjectsOfType<PlantDestroyer>())
            {
                if (Vector3.Distance(targetPos, plantDestroyers.transform.position) < 5f)
                {
                    canWalk = false;
                }
            }

            if (canWalk)
            {
                aiAgent.SetDestination(targetPos);

                yield return new WaitForSeconds(Random.Range(wanderPauseMax, wanderPauseMax));
                isRunning = false;
            }
        }
    }

    // Determines whether there is a clone currently spawned in the scene.
    private void IsCloneSpawned()
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
    private void FollowClone()
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
    private void LookAtClone()
    {
        var target = GameObject.FindWithTag("Clone");
            
        Quaternion lookOnLook = Quaternion.LookRotation(target.transform.position - transform.position);
 
        transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * 10f);
    }

    // Sets Faceless AI target location depending on which point it just passed through.
    // Saves the last target position.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AIStop"))
        {
            crytalPos = other.transform.position;
            aiAgent.SetDestination(crytalPos);
            isFollowingCrystal = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
