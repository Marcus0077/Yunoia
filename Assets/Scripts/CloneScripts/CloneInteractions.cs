using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;

public class CloneInteractions : MonoBehaviour
{
    // Script references
    public BasicMovement basicMovementPlayer;
    public BasicMovement basicMovementClone;
    public AbilityPush cloneAbilityPush;
    public Lever lever;
    public Door door;
    public LimitedMovementCam limitedMovementCam;

    // Player game object reference.
    public GameObject Player;
    
    // Input variables.
    public PlayerControls playerControls;
    public InputAction switchPlaces;
    public InputAction press;
    
    // Interactable Booleans.
    public bool canPressLever;
    public bool canPressDoor;

    // Particle Variables.
    public GameObject duplicateParticles;
    private float footPos;

    // Animation variables.
    public Animator anim;
    public Animator animCover;

    // Get references and initialize variables when clone is instantiated.
    void Awake()
    {
        playerControls = new PlayerControls();
        
        Player = GameObject.FindWithTag("Player");

        cloneAbilityPush = this.GetComponent<AbilityPush>();
        cloneAbilityPush.restored = Player.GetComponent<AbilityPush>().restored;
        basicMovementPlayer = Player.GetComponent<BasicMovement>();
        basicMovementClone = this.GetComponent<BasicMovement>();
        limitedMovementCam = FindObjectOfType<LimitedMovementCam>();

        Physics.IgnoreCollision(this.GetComponent<Collider>(), Player.GetComponent<Collider>(), true);

        canPressLever = false;

        footPos = duplicateParticles.transform.position.y;

        StartCoroutine(DestroyCloneParticles());
    }
    

    // Called each frame.
    private void Update()
    {
        SwitchPlaces();
        CheckLeverPress();
        CheckDoorPress();
        
    }

    // Called between frames.
    private void FixedUpdate()
    {
        // Force clone particles to follow the clone's feet as long as the particles still exist.
        if (duplicateParticles != null)
        {
            duplicateParticles.transform.position = new Vector3(duplicateParticles.transform.position.x, footPos,
                duplicateParticles.transform.position.z);
        }
    }

    // Destroy the clone particles 2 seconds after it spawns.
    private IEnumerator DestroyCloneParticles()
    {
        yield return new WaitForSeconds(2f);
        
        Destroy(duplicateParticles);
    }

    // Checks if clone can activate a lever and whether they activated it.
    // If they can activate it and it is not activated yet, allow them to
    // activate it.
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

    // Checks if clone can activate a door and whether they activated it.
    // If they can activate it and it is not activated yet, allow them to
    // activate it.
    void CheckDoorPress()
    {
        if (canPressDoor && press.WasPressedThisFrame())
        {
            if (door != null && door.canInteract && this.GetComponent<BasicMovement>().canMove)
            {
                door.Open();
            }
        }
    }

    // Switch clone and player control and camera depending on which is
    // currently in control.
    private void SwitchPlaces()
    {
        if (switchPlaces.WasReleasedThisFrame())
        {
            // If we are currently in control of the clone, 
            // give control to the player, and switch the camera to the player.
            // Destroys and disables clone abilities in the process.
            if (basicMovementPlayer.canMove == false)
            {
                limitedMovementCam.GetCurrentCameraData(basicMovementPlayer.curRoom);
                limitedMovementCam.SetCurrentPlayer(basicMovementPlayer.GameObject());
                    
                if (anim != null)
                {
                    anim.SetBool("isClone", false);
                    animCover.SetBool("isClone", false);
                }

                basicMovementPlayer.canMove = true;
                basicMovementClone.canMove = false;
                
                this.GetComponent<Grapple>().DestroyHook();
                this.GetComponent<AbilityPush>().DestroyShape();

                this.GetComponent<Grapple>().enabled = false;
                this.GetComponent<AbilityPush>().enabled = false;

                Player.GetComponent<Grapple>().enabled = true;
                Player.GetComponent<AbilityPush>().enabled = true;
            }
            // If we are currently in control of the player, 
            // give control to the clone, and switch the camera to the clone.
            // Destroys and disables player abilities in the process.
            else if (basicMovementClone.canMove == false)
            {
                limitedMovementCam.GetCurrentCameraData(basicMovementClone.curRoom);
                limitedMovementCam.SetCurrentPlayer(basicMovementClone.GameObject());
                
                if (anim != null)
                {
                    anim.SetBool("isClone", true);
                    animCover.SetBool("isClone", true);
                }

                basicMovementPlayer.canMove = false;
                basicMovementClone.canMove = true;
                
                Player.GetComponent<Grapple>().DestroyHook();
                Player.GetComponent<AbilityPush>().DestroyShape();

                this.GetComponent<Grapple>().enabled = true;
                this.GetComponent<AbilityPush>().enabled = true;

                Player.GetComponent<Grapple>().enabled = false;
                Player.GetComponent<AbilityPush>().enabled = false;
            }
        }
    }
    
    // Checks if clone is in a lever or door trigger(s).
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

    // Enable input action map controls.
    private void OnEnable()
    {
        switchPlaces = playerControls.SummonClone.SwitchPlaces;
        switchPlaces.Enable();

        press = playerControls.Interaction.Press;
        press.Enable();
    }
    
    // Disable input action map controls.
    private void OnDisable()
    {
        switchPlaces.Disable();
        press.Disable();
    }
}
