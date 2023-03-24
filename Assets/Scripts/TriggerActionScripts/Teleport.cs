using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform teleportTarget;
    public GameObject player, terrain, ravine;

    void OnTriggerEnter (Collider collider)
    {
        player.transform.position = teleportTarget.transform.position;
        ravine.SetActive(false);
        terrain.SetActive(true);
    }
}
