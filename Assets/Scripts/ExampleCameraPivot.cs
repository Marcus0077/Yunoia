using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleCameraPivot : MonoBehaviour
{
    public SmoothCameraFollow cameraFollower;
    [Range(-20f, 20f)]
    public float offsetX;
    [Range(-20f, 20f)]
    public float offsetY;
    [Range(-20f, 20f)]
    public float offsetZ;
    [Range(0.05f, 2f)]
    public float smoothSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            GameObject hit = contact.otherCollider.gameObject;
            if (hit.tag == "Player")
            {
                cameraFollower.offsetX = offsetX;
                cameraFollower.offsetY = offsetY;
                cameraFollower.offsetZ = offsetZ;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
