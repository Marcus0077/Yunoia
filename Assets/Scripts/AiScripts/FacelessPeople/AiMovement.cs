using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AiMovement : MonoBehaviour
{
    // Audio variables.
    public AudioSource audioSource;
    public AudioClip detectionSound;
    
    // NavMeshAgent reference.
    [SerializeField] private NavMeshAgent aiAgent;

    // Target positioning variables.
    private Vector3 targetPos;
    private Vector3 clonePos;
    private float distanceBetweenClone;
    
    // AI x Crystal Pressure Plate variables.
    private bool isStoppedByCrystal;
    private bool isFollowingCrystal;
    private Vector3 crytalPos;
    public bool canAiMove;

    // AI Wandering variables.
    private float wanderDistance;
    private IEnumerator wanderCoroutine;
    private bool isRunning;
    
    [Range(1.0f, 10.0f)]
    public float wanderDistanceMin; // Minimum distance an AI can wander.
    
    [Range(1.0f, 10.0f)]
    public float wanderDistanceMax; // Maximum distance an AI can wander.
    
    [Range(0.5f, 5.0f)]
    public float wanderPauseMin; // Minimum amount of time an AI can wait before wandering again.

    [Range(0.5f, 5.0f)] 
    public float wanderPauseMax; // Maximum amount of time an AI can wait before wandering again.
    
    // Get references and initialize variables when the AI is initialised.
    private void Awake()
    {
        aiAgent = this.GetComponent<NavMeshAgent>();

        isRunning = false;
        isStoppedByCrystal = false;
        isFollowingCrystal = false;
        
        distanceBetweenClone = 100f;
    }
    
    // This method is called between frames.
    private void FixedUpdate()
    {
        // If the AI can move, check to see if the clone is spawned & close by.
        if (canAiMove)
        {
            IsCloneSpawned();
            DetermineCloneDistance();
        }

        // If the AI has entered a crystal pressure plate, is moving towards the middle of it,
        // and is close enough to the middle of the pressure plate to stop, then stop the AI.
        if (!isStoppedByCrystal && isFollowingCrystal && Vector3.Distance(this.transform.position, crytalPos) < 0.025)
        {
            isStoppedByCrystal = true;
        }
    }

    // LEFT OFF HERE FOR CODE CLEANUP.
    private void DetermineCloneDistance()
    {
        if (distanceBetweenClone < 4 && !isFollowingCrystal)
        {
            
            ChaseClone();
            
            //audioSource.PlayOneShot(detectionSound); needs cd
            
            if (isRunning)
            {
                StopCoroutine(wanderCoroutine);
                isRunning = false;
            }
        }
        else if (!isStoppedByCrystal && !isFollowingCrystal && !isRunning)
        {
            aiAgent.isStopped = false;
            
            wanderCoroutine = Wander();
            StartCoroutine(wanderCoroutine);
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

                walked = true;
            }
        }
        
        yield return new WaitForSeconds(Random.Range(wanderPauseMin, wanderPauseMax));
        
        Debug.Log("found spot");
        
        isRunning = false;
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
            StopCoroutine(Wander());
            
            crytalPos = other.transform.position;
            aiAgent.SetDestination(crytalPos);
            isFollowingCrystal = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
