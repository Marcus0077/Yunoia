using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class LevelSwitchManager : MonoBehaviour
{

    public Rigidbody Player;

    public List<Transform> LevelStarts;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MutedGrappleEnd"))
        {
            Player.transform.position = LevelStarts[0].position;
        }
        else if (other.CompareTag("MutedPushEnd"))
        {
            Player.transform.position = LevelStarts[1].position;
        }
        if (other.CompareTag("MutedCloneEnd"))
        {
            Player.transform.position = LevelStarts[2].position;
        }
    }
}
