using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class CloneInteractions : MonoBehaviour
{
    // Script references
    public BasicMovement basicMovementPlayer;
    public BasicMovement basicMovementClone;
    public SmoothCameraFollow smoothCameraFollow;
    public SummonClone summonClone;
    public ExitClone exitClone;
    public CombatHandler combatHandler;
    public AbilityPush cloneAbilityPush;
    public Lever lever;
    public Door door;

    // Player game object reference.
    public GameObject Player;
    
    // Input variables
    PlayerControls playerControls;
    public InputAction switchPlaces;
    public InputAction press;

    // Clone version bool.
    public bool cloneRestored;

    public bool canPress;
    public bool canPressDoor;

    public GameObject duplicateParticles;
    private float footPos;

    //Clone UI Experiment here
    public Animator anim;

    // Get references and initialize variables when clone is instantiated.
    void Awake()
    {
        Player = GameObject.FindWithTag("Player");

        cloneAbilityPush.restored = Player.GetComponent<AbilityPush>().restored;
        
        basicMovementPlayer = Player.GetComponent<BasicMovement>();
        basicMovementClone = this.GetComponent<BasicMovement>();
        smoothCameraFollow = FindObjectOfType<SmoothCameraFollow>();
        summonClone = FindObjectOfType<SummonClone>();
        exitClone = FindObjectOfType<ExitClone>();
        combatHandler = FindObjectOfType<CombatHandler>();

        Physics.IgnoreCollision(this.GetComponent<Collider>(), Player.GetComponent<Collider>(), true);

        smoothCameraFollow.target = this.transform;
        
        playerControls = new PlayerControls();
        
        combatHandler.cloneHP = 3;
        combatHandler.healthText.text = "Clone Health: " + combatHandler.cloneHP + "/3";

        cloneRestored = true;
        canPress = false;

        footPos = duplicateParticles.transform.position.y;
    }
    

    // Called each frame.
    private void Update()
    {
        SwitchPlaces();

        if (canPress && press.WasPressedThisFrame())
        {
            if (lever != null && !lever.isActivated && this.GetComponent<BasicMovement>().canMove)
            {
                lever.isActivated = true;
            }
        }
        else if (canPressDoor && press.WasPressedThisFrame())
        {
            if (door != null && door.canInteract)
            {
                door.Open();
            }
        }
    }

    private void FixedUpdate()
    {
        duplicateParticles.transform.position = new Vector3(duplicateParticles.transform.position.x, footPos, duplicateParticles.transform.position.z);
    }

    // Switch clone and player control depending on which is
    // currently in control.
    private void SwitchPlaces()
    {
        if (switchPlaces.WasPressedThisFrame())
        {
            if (basicMovementPlayer.canMove == false)
            {
                anim.SetBool("isClone", false);

                basicMovementPlayer.canMove = true;
                basicMovementClone.canMove = false;
                
                this.GetComponent<Grapple>().DestroyHook();
                this.GetComponent<AbilityPush>().DestroyShape();

                this.GetComponent<Grapple>().enabled = false;
                this.GetComponent<AbilityPush>().enabled = false;

                Player.GetComponent<Grapple>().enabled = true;
                Player.GetComponent<AbilityPush>().enabled = true;

                smoothCameraFollow.target = basicMovementPlayer.playerRB.transform;
            }
            else if (basicMovementClone.canMove == false)
            {
                anim.SetBool("isClone", true);

                basicMovementPlayer.canMove = false;
                basicMovementClone.canMove = true;
                
                Player.GetComponent<Grapple>().DestroyHook();
                Player.GetComponent<AbilityPush>().DestroyShape();

                this.GetComponent<Grapple>().enabled = true;
                this.GetComponent<AbilityPush>().enabled = true;

                Player.GetComponent<Grapple>().enabled = false;
                Player.GetComponent<AbilityPush>().enabled = false;

                smoothCameraFollow.target = this.transform;
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
