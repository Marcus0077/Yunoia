using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiOrbs : MonoBehaviour
{
    NavMeshAgent aiController;
    [SerializeField]
    GameObject player, stickyOrb;
    [SerializeField]
    float detectDistance;
    // Start is called before the first frame update
    void Start()
    {
        aiController = GetComponent<NavMeshAgent>();
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
            GameObject orb = Instantiate(stickyOrb, transform.position, transform.rotation, player.transform);
            Destroy(gameObject);
        }
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
                    aiController.destination = player.transform.position;
                }
            }
        }
    }
    
    void Update()
    {
        DistancePlayer();
    }
}
