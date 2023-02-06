using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform teleportTarget;
    public GameObject player;

    void OnTriggerEnter (Collider collider)
    {
        player.transform.position = teleportTarget.transform.position;
    }
}
