using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiOrbs : Pushable
{
    //NavMeshAgent aiController;
    [SerializeField]
    GameObject stickyOrb;
    public GameObject player;
    // Detects if a player or clone was in range at any point
    bool seenOnce = false;
    // Make sure only 1 delay coroutine is running
    Coroutine delay = null;
    [SerializeField]
    float detectDistance, movespeed, timeToDelay;
    public float animationDetectDistance;

    // Start is called before the first frame update
    void Start()
    {
        //aiController = GetComponent<NavMeshAgent>();
        if(player == null)
            player = Object.FindObjectsOfType<AbilityPush>()[0].gameObject;
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("a");
    //    foreach (ContactPoint contact in collision.contacts)
    //    {
    //        Debug.DrawRay(contact.point, contact.normal, Color.white);
    //    }
    //}

    // On touching player replace self with stickyOrb
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            AiOrbsSticky orb = Instantiate(stickyOrb, transform.position, transform.rotation).GetComponent<AiOrbsSticky>();
            orb.player = player;
            Destroy(gameObject);
        }
    }

    // Change to a stickyOrb object to disable movement temporarily
    public override bool Pushed(Vector3 force, int chargeLevel, int totalCharges, GameObject pusher)
    {
        if(chargeLevel >= reqChargeLevel)
        {
            AiOrbsSticky orb = Instantiate(stickyOrb, transform.position, transform.rotation).GetComponent<AiOrbsSticky>();
            orb.player = player;
            orb.Pushed(force, chargeLevel, totalCharges, pusher);
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    public override void Awake()
    {
        base.Awake();
        seenOnce = false;
    }

    // Detects distance from player or clone and chases
    void DistancePlayer()
    {
        if(animationDetectDistance > detectDistance)
        {
            detectDistance = animationDetectDistance;
        }
        RaycastHit hit;
        GameObject closer = null;
        float distance = 100;
        foreach(AbilityPush play in Object.FindObjectsOfType<AbilityPush>())
        {
            if (Physics.Raycast(transform.position, play.gameObject.transform.position - transform.position, out hit, Mathf.Infinity, LayerMask.GetMask("Player", "Clone")))
            {
                if (hit.transform.gameObject.GetComponent<AbilityPush>() != null) // Chase any object that can push (player or clone) in line of sight
                {
                    if(!seenOnce) // If player has never been in chase range, delay chasing until timeToDelay has elapsed
                    {
                        if (delay == null)
                        {
                            delay = StartCoroutine(DelayChase(timeToDelay));
                        }
                        return;
                    }
                    if (hit.distance < detectDistance)
                    {
                        if (distance > hit.distance)
                        {
                            closer = hit.transform.gameObject;
                        }
                        //aiController.destination = player.transform.position;
                    }
                }
            }
        }
        if (closer != null)
        {
            //Debug.Log(closer);
            player = closer;
            transform.position = Vector3.Lerp(transform.position, closer.transform.position, Time.deltaTime * movespeed);
            transform.LookAt(closer.transform);
        }
    }

    IEnumerator DelayChase(float time)
    {
        yield return new WaitForSeconds(time);
        seenOnce = true;
        delay = null;
    }

    void Update()
    {
        DistancePlayer();
    }
}
