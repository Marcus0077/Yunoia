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
    [SerializeField]
    float detectDistance, movespeed;

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

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            AiOrbsSticky orb = Instantiate(stickyOrb, transform.position, transform.rotation).GetComponent<AiOrbsSticky>();
            orb.player = player;
            Destroy(gameObject);
        }
    }

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
    }

    void DistancePlayer()
    {
        RaycastHit hit;
        GameObject closer = null;
        float distance = 100;
        foreach(AbilityPush play in Object.FindObjectsOfType<AbilityPush>())
        {
            if (Physics.Raycast(transform.position, play.gameObject.transform.position - transform.position, out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.GetComponent<AbilityPush>() != null)
                {
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
            Debug.Log(closer);
            player = closer;
            transform.position = Vector3.Lerp(transform.position, closer.transform.position, Time.deltaTime * movespeed);
            transform.LookAt(closer.transform);
        }
    }

    void Update()
    {
        DistancePlayer();
    }
}
