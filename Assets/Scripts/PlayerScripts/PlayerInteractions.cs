using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteractions : MonoBehaviour
{
    public bool canPress;

    public Lever lever;
    
    PlayerControls playerControls;
    public InputAction switchPlaces;
    public InputAction press;

    private void Start()
    {
        playerControls = new PlayerControls();
        
        press = playerControls.Interaction.Press;
        press.Enable();
        
        canPress = false;
    }

    private void Update()
    {
        if (canPress && press.WasPressedThisFrame() && !lever.isActivated && this.GetComponent<BasicMovement>().canMove)
        {
            lever.isActivated = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lever"))
        {
            lever = other.GetComponent<Lever>();
        }
    }

    private void OnDisable()
    {
        press = playerControls.Interaction.Press;
    }
}