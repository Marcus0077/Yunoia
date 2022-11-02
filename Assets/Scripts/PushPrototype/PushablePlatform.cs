using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushablePlatform : Pushable
{
    Vector3 prevPosition;

    void OnTriggerEnter(Collider other)
    {
        GameObject hit = other.gameObject;
        if (hit.tag == "Player" || hit.tag == "Clone")
        {
            prevPosition = transform.position;
        }
    }

    void OnTriggerStay(Collider other)
    {
        GameObject hit = other.gameObject;
        if (hit.tag == "Player" || hit.tag == "Clone")
        {
            hit.transform.position -= prevPosition - transform.position;
            prevPosition = transform.position;
        }
    }
}
