using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaverMovement : MonoBehaviour

{
    public Transform[] waypoints;   // Array of waypoints for the object to move through
    public float speed = 5f;        // Speed at which the object moves
    private int currentWaypoint = 0; // Index of the current waypoint
    private Vector3 direction;      // Direction the object is moving
    private Quaternion rotation;   // Rotation of the object

    void Start()
    {
        // Set the initial direction to the first waypoint
        direction = (waypoints[currentWaypoint].position - transform.position).normalized;

        // Set the initial rotation to face the direction of movement
        rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }

    void Update()
    {
        // Move the object towards the current waypoint
        transform.position += direction * speed * Time.deltaTime;

        // If the object is close enough to the current waypoint, move to the next one
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 0.1f)
        {
            currentWaypoint++;

            // If the object has reached the final waypoint, reset to the first waypoint
            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }

            // Set the new direction and rotation based on the new waypoint
            direction = (waypoints[currentWaypoint].position - transform.position).normalized;
            rotation = Quaternion.LookRotation(direction);
        }

        // Rotate the object to face the direction of movement
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
    }
}