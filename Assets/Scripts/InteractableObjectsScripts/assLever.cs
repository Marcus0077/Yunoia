using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class assLever : MonoBehaviour
{
    // Input Variables
    public PlayerControls playerControls;
    public InputAction press;
    
    // Ramp Game Object
    public GameObject ramp;

    // Can we pull or nah?
    private bool canPull;
    
    // Start is called before the first frame update
    void Start()
    {
        playerControls = new PlayerControls();
        press = playerControls.Interaction.Press;
        press.Enable();
        
        canPull = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canPull && press.IsPressed())
        {
            ramp.transform.rotation = Quaternion.Euler(new Vector3(ramp.transform.rotation.x, ramp.transform.rotation.y, -34f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Clone"))
        {
            canPull = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Clone"))
        {
            canPull = false;
        }
    }
}
