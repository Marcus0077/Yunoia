using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaverHouseDoorRotate : MonoBehaviour
{
    public float activationDistance = 2f;
    public float rotationSpeed = 1f;
    private Transform playerTransform;
    private bool isPlayerClose = false;
    private Quaternion startRotation;
    private Quaternion targetRotation = Quaternion.Euler(0, 180, 0); // Target rotation

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        startRotation = transform.rotation; // Record the starting rotation
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerClose)
        {
            Vector3 directionToPlayer = playerTransform.position - transform.position;
            if (directionToPlayer.magnitude < activationDistance)
            {
                // Rotate the object towards the target rotation
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    // Called when the player enters the object's trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerClose = true;
        }
    }

    // Called when the player exits the object's trigger zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerClose = false;
        }
    }

    // Draw a wire sphere around the object's trigger zone for visual debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activationDistance);
    }
}