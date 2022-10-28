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
}
