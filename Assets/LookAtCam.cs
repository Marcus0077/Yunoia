using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor.Experimental.RestService;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    private Transform playerPos;
    private Vector3 playerUpVector;
        
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        playerUpVector = new Vector3(transform.position.x, playerPos.position.y, transform.position.z);
        transform.LookAt(playerPos);
        transform.position = playerUpVector;
    }
}
