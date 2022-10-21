using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiOrbs : Pushable
{
    //NavMeshAgent aiController;
    [SerializeField]
    GameObject player, stickyOrb;
    [SerializeField]
    float detectDistance, speed;
    // Start is called before the first frame update
    void Start()
    {
        //aiController = GetComponent<NavMeshAgent>();
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
            GameObject orb = Instantiate(stickyOrb, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public override void Pushed(Vector3 force, int chargeLevel, int totalCharges)
    {
        AiOrbsSticky orb = Instantiate(stickyOrb, transform.position, transform.rotation).GetComponent<AiOrbsSticky>();
        orb.Pushed(force, chargeLevel, totalCharges);
        Destroy(gameObject);
    }

    public override void Awake()
    {
        base.Awake();
    }

    void DistancePlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject.GetComponent<AbilityPush>() != null)
            {
                if(hit.distance < detectDistance)
                {
                    //aiController.destination = player.transform.position;
                    transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime * speed);
                }
            }
        }
    }

    void Update()
    {
        DistancePlayer();
    }
}
