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
    public LevelSwitchManager levelSwitchManager;
    public AbilityPush cloneAbilityPush;

    // Player game object reference.
    public GameObject Player;
    
    // Input variables
    PlayerControls playerControls;
    public InputAction switchPlaces;

    // Temp Object References
    public bool isOnTrigger2;
    public GameObject Wall_1;
    public GameObject Blocker2;
    public GameObject Blocker3;
    
    public Vector3 blocker3InPos;
    public Vector3 blocker3OutPos;

    // Clone version bool.
    public bool cloneRestored;

    // Get references and initialize variables when clone is instantiated.
    void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        
        basicMovementPlayer = Player.GetComponent<BasicMovement>();
        basicMovementClone = this.GetComponent<BasicMovement>();
        smoothCameraFollow = FindObjectOfType<SmoothCameraFollow>();
        summonClone = FindObjectOfType<SummonClone>();
        exitClone = FindObjectOfType<ExitClone>();
        combatHandler = FindObjectOfType<CombatHandler>();
        
        levelSwitchManager = FindObjectOfType<LevelSwitchManager>();
        
        Physics.IgnoreCollision(this.GetComponent<Collider>(), Player.GetComponent<Collider>(), true);

        Wall_1 = GameObject.FindWithTag("GoodWall1");
        Blocker2 = GameObject.FindWithTag("Blocker2");
        Blocker3 = GameObject.FindWithTag("Blocker3");
        
        blocker3InPos = Blocker3.transform.position;
        blocker3OutPos = new Vector3(blocker3InPos.x - 3f, blocker3InPos.y,
            blocker3InPos.z);

        smoothCameraFollow.target = this.transform;
        
        playerControls = new PlayerControls();
        
        combatHandler.cloneHP = 3;
        combatHandler.healthText.text = "Clone Health: " + combatHandler.cloneHP + "/3";

        cloneRestored = true;
        isOnTrigger2 = false;
        
        if (levelSwitchManager.pushRestored == true)
        {
            cloneAbilityPush.restored = true;
        }
        else
        {
            cloneAbilityPush.restored = false;
        }
    }
    

    // Called each frame.
    private void Update()
    {
        SwitchPlaces();
        CheckRestoredWalls();
    }

    // Switch clone and player control depending on which is
    // currently in control.
    private void SwitchPlaces()
    {
        if (switchPlaces.WasPressedThisFrame())
        {
            if (basicMovementPlayer.canMove == false)
            {
                basicMovementPlayer.canMove = true;
                basicMovementClone.canMove = false;

                this.GetComponent<Grapple>().enabled = false;
                this.GetComponent<AbilityPush>().enabled = false;

                Player.GetComponent<Grapple>().enabled = true;
                Player.GetComponent<AbilityPush>().enabled = true;

                smoothCameraFollow.target = basicMovementPlayer.playerRB.transform;
            }
            else if (basicMovementClone.canMove == false)
            {
                basicMovementPlayer.canMove = false;
                basicMovementClone.canMove = true;

                this.GetComponent<Grapple>().enabled = true;
                this.GetComponent<AbilityPush>().enabled = true;

                Player.GetComponent<Grapple>().enabled = false;
                Player.GetComponent<AbilityPush>().enabled = false;

                smoothCameraFollow.target = this.transform;
            }
        }
    }

    // Enable input action map controls.
    private void OnEnable()
    {
        switchPlaces = playerControls.SummonClone.SwitchPlaces;
        switchPlaces.Enable();
    }
    
    // Disable input action map controls.
    private void OnDisable()
    {
        switchPlaces.Disable();
    }

    // Determines if clone is on a pressure plate in prototype (temporary).
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlockerTrigger3"))
        {
            Blocker3.transform.position = blocker3OutPos;
        }

        if (other.CompareTag("BlockerTrigger2"))
        {
            isOnTrigger2 = true;
            
            Blocker2.GetComponent<MeshRenderer>().enabled = false;
            Blocker2.GetComponent<Collider>().enabled = false;
        }
    }
    
    // Determines if clone has exited a pressure plate in prototype (temporary).
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BlockerTrigger3"))
        {
            Blocker3.transform.position = blocker3InPos;
        }

        if (other.CompareTag("BlockerTrigger2"))
        {
            isOnTrigger2 = false;
            
            Blocker2.GetComponent<MeshRenderer>().enabled = true;
            Blocker2.GetComponent<Collider>().enabled = true;
        }
    }

    // Allows clone to phase through the translucent green wall in prototype (temporary).
    void CheckRestoredWalls()
    {
        Physics.IgnoreCollision(Wall_1.GetComponent<Collider>(), this.GetComponent<Collider>(), true);
    }
}
