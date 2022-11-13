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
    public bool canPressDoor;

    public Lever lever;
    public Door door;

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
        if (canPress && press.WasPressedThisFrame())
        {
            if (lever != null && !lever.isActivated && this.GetComponent<BasicMovement>().canMove)
            {
                lever.isActivated = true;
            }
        }
        else if(canPressDoor && press.WasPressedThisFrame())
        {
            if (door != null && door.canInteract)
            {
                door.Open();
            }
        }
    }

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

    private void OnDisable()
    {
        press = playerControls.Interaction.Press;
    }
}
