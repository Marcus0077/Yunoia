using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaverMovement : MonoBehaviour

{
    public GameObject[] waypoints;
    int current = 0;
    float rotSpeed;
    float WPradius = 1; 
    public float speed;

    void Update()
    {
        if (Vector3.Distance(waypoints[current].transform.position, transform.position) < WPradius)
        {
            current++;
            if (current >= waypoints.length)
            {
                current = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.postion, waypoints[current].transform.position, Time.deltaTime * speed); 

    } 

}