using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEditor.Experimental;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    private Transform playerPos;
    private Transform clonePos;
    
    private Vector3 targetUpVector;
    public Quaternion lookRotation;
    private float speed = 2;
        
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerPos.GetComponent<BasicMovement>().canMove)
        {
            lookRotation = Quaternion.LookRotation(playerPos.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
            
            targetUpVector = new Vector3(transform.position.x, playerPos.position.y + 2f, transform.position.z);
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("Clone") != null)
            {
                clonePos = GameObject.FindGameObjectWithTag("Clone").transform;

                lookRotation = Quaternion.LookRotation(clonePos.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
                
                targetUpVector = new Vector3(transform.position.x, clonePos.position.y + 2f, transform.position.z);
            }
        }

        transform.position = targetUpVector;
    }
}
