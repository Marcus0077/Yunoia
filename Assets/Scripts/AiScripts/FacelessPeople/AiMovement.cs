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
    public bool isStoppedByCrystal;
    public bool isFollowingCrystal;
    private Vector3 crytalPos;
    public bool canAiMove;

    // AI Wandering variables.
    private float wanderDistance;
    public IEnumerator wanderCoroutine;
    public bool isWanderRunning;
    
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

        isWanderRunning = false;
        isStoppedByCrystal = false;
        isFollowingCrystal = false;
        
        distanceBetweenClone = 100f;
    }
    
    // Called between frames.
    private void FixedUpdate()
    {
        // If the AI can move, check to see if the clone is spawned & close by.
        if (canAiMove && !isStoppedByCrystal && !isFollowingCrystal)
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

    // Determines if clone is close enough to chase. If not, the AI will wander.
    private void DetermineCloneDistance()
    {
        // If the clone is close enough and the AI is not on a crystal
        // pressure plate, then chase the clone.
        if (distanceBetweenClone <= 4 && !isFollowingCrystal)
        {
            if (GameObject.FindWithTag("Clone") != null)
            {
                ChaseClone();
            }

            //audioSource.PlayOneShot(detectionSound); needs cd
            
            // If the AI was previously wandering, stop them so that they
            // can chase the clone.
            if (isWanderRunning)
            {
                StopCoroutine(wanderCoroutine);
                isWanderRunning = false;
            }
        }
        // If the AI is not close to the clone, not on a crystal pressure plate,
        // and not currently wandering, allow AI to wander.
        else if (!isStoppedByCrystal && !isFollowingCrystal && !isWanderRunning && distanceBetweenClone > 4)
        {
            aiAgent.isStopped = false;
            
            wanderCoroutine = Wander();
            StartCoroutine(wanderCoroutine);
        }
        else if (!isWanderRunning)
        {
            Debug.Log("??????");
        }
    }

    // Sets Faceless AI to look at and chase the clone.
    private void ChaseClone()
    {
        if (GameObject.FindWithTag("Clone") != null && !isWanderRunning)
        {
            LookAtClone();
            FollowClone();
        }
    }

    // Finds a random point within a radius that is 'distance' length from the AI's current position
    // and that is a valid point on the NavMesh Plane, according to what NavMesh Area Mask(s) the AI
    // is allowed to travel on, and returns that point.
    // TDLR: Finds a random point within a circle for the AI to travel to.
    private Vector3 RandomNavSphere(Vector3 origin, float distance, int areaMask)
    {
        Vector3 randomDirection = new Vector3(UnityEngine.Random.insideUnitSphere.x,
            0f, UnityEngine.Random.insideUnitSphere.z) * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, areaMask);

        return navHit.position;
    }

    // Finds a random point for the AI to walk to, checks to make sure that the point
    // is not close to any crystal pressure plates, and makes AI walk to that point.
    public IEnumerator Wander()
    {
        isWanderRunning = true;
        
        // Determines if a point is valid or not.
        bool canWalk;
        
        // True if a valid spot is point is found and AI
        // is walking there.
        bool walked = false;
        
        // Use a random distance value for AI to walk to.
        wanderDistance = Random.Range(wanderDistanceMin, wanderDistanceMax);

        // While the AI has not found a valid point, look for a valid point.
        while (!walked)
        {
            canWalk = true; // Will remain true if the potential spot is valid.
            
            // Find a random spot using this AI's current position,
            // the random distance value, and a default Area Mask.
            targetPos = RandomNavSphere(this.transform.position, wanderDistance, 1);

            // Make sure that the potential spot is not too close to any crystal pressure plates.
            // If so, set that point as invalid.
            foreach (var plantDestroyers in GameObject.FindObjectsOfType<PlantDestroyer>())
            {
                if (Vector3.Distance(targetPos, plantDestroyers.transform.position) < 2f)
                {
                    canWalk = false;
                }
            }

            // If a valid point was found, set the AI's target destination to that
            // point and break out of the loop.
            if (canWalk)
            {
                aiAgent.SetDestination(targetPos);

                walked = true;
            }
        }
        
        // Wait a random amount of time before wandering again.
        yield return new WaitForSeconds(Random.Range(wanderPauseMin, wanderPauseMax));
        
        isWanderRunning = false;
    }

    // Determines whether there is a clone currently spawned in the scene.
    private void IsCloneSpawned()
    {
        // If there is a clone spawned, find it's position and distance from this AI.
        if (GameObject.FindWithTag("Clone") != null)
        {
            clonePos = GameObject.FindWithTag("Clone").transform.position;
            distanceBetweenClone = Vector3.Distance(this.transform.position, clonePos);
        }
        // If not, set the distance between the AI and Clone to a high number.
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

    // If the AI has reached a crystal pressure plate, make them stop what they
    // are doing, and set their target destination to the center of the pressure plate.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AIStop"))
        {
            if (other.GetComponent<PlantDestroyer>().coAI == this.GameObject())
            {
                isFollowingCrystal = true;
                
                if (isWanderRunning)
                {
                    isWanderRunning = false;
                    StopCoroutine(wanderCoroutine);
                }

                crytalPos = other.transform.position;
                aiAgent.SetDestination(crytalPos);
            }
        }
    }
}
