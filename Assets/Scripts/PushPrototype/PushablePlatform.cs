using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushablePlatform : Pushable
{
    
    void OnTriggerEnter(Collider other)
    {
        GameObject hit = other.gameObject;
        if (hit.tag == "Player" || hit.tag == "Clone")
        {
            hit.transform.SetParent(transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameObject hit = other.gameObject;
        if (hit.tag == "Player" || hit.tag == "Clone")
        {
            hit.transform.SetParent(null);
        }
    }

    void FixedUpdate()
    {
        if(transform.childCount > 0)
        {
            if (transform.GetChild(0).tag == "Player")
            {
                if (transform.GetChild(0).GetComponent<BasicMovementPlayer>().isGrounded)
                {
                    transform.GetChild(0).transform.position = new Vector3(transform.position.x, transform.GetChild(0).transform.position.y, transform.position.z);
                }
            }
            else if (transform.GetChild(0).tag == "Clone")
            {
                if (transform.GetChild(0).GetComponent<BasicMovementClone>().isGrounded)
                {
                    transform.GetChild(0).transform.position = new Vector3(transform.position.x, transform.GetChild(0).transform.position.y, transform.position.z);
                }
            }
        }
        CapSpeed();
    }
}
