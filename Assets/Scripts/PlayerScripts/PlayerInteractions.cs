using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteractions : MonoBehaviour
{
    // Script references.
    public Lever lever;
    public Door door;
    
    // Input Variables
    public PlayerControls playerControls;
    public InputAction switchPlaces;
    public InputAction press;

    // Interactable variables.
    public bool canPressLever;
    public bool canPressDoor;

    // Get references and initialize variables when player spawns.
    private void Awake()
    {
        playerControls = new PlayerControls();
        
        press = playerControls.Interaction.Press;
        press.Enable();
        
        canPressLever = false;
    }

    // Called each frame.
    private void Update()
    {
        CheckLeverPress();
        CheckDoorPress();
        
    }
    
    // Checks if player can activate a lever and whether they activated it.
    void CheckLeverPress()
    {
        if (canPressLever && press.WasPressedThisFrame())
        {
            if (lever != null && !lever.isActivated && this.GetComponent<BasicMovement>().canMove)
            {
                lever.isActivated = true;
            }
        }
    }

    // Checks if player can activate a door and whether they activated it.
    void CheckDoorPress()
    {
        if(canPressDoor && press.WasPressedThisFrame())
        {
            if (door != null && door.canInteract)
            {
                door.Open();
            }
        }
    }

    // Checks if player is in a lever or door trigger(s).
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lever"))
        {
            lever = other.GetComponent<Lever>();
        }
        else if (other.CompareTag("Door"))
        {
            door = other.GetComponent<Door>();
        }
    }

    // Disable input action map controls.
    private void OnDisable()
    {
        press = playerControls.Interaction.Press;
    }
}
