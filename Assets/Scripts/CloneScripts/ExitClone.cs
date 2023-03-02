using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ExitClone : MonoBehaviour
{
    // Audio variables.
    public AudioSource audioSource;
    public AudioClip beginDestructSound;
    public AudioClip destructSound;
    
    // Temporary Despawn Timer UI.
    public TextMeshProUGUI despawnTimerText;
    
    // Animation variables.
    public Animator anim;
    public Animator animCover;

    // Script references
    private BasicMovement basicMovementPlayer;
    private BasicMovement basicMovementClone;
    private SummonClone summonClone;
    private CombatHandler combatHandler;
    private CloneInteractions cloneInteractions;
    private PressurePlate pressurePlate;
    private Lever lever;
    private Door door;
    public LimitedMovementCam limitedMovementCam;
    
    // Player reference.
    public GameObject Player;

    // Input variables.
    PlayerControls summonControls;
    private InputAction exitClone;

    // Despawn clone variables.
    public bool despawnClone;
    public float despawnCloneTimer;
    
    // Interactable trigger variables.
    private bool isOnPressurePlate;
    private bool isOnLever;
    private bool isOnDoor;
    
    // Layer Mask Variables
    public LayerMask pPlateLayer;

    // Clone timer variables.
    public float cloneActiveTimer;
    private bool isRunning;
    public float Timer
    {
        get { return cloneActiveTimer; }
        set { cloneActiveTimer = value; }
    }

    // Get references and initialize variables when clone is instantiated.
    void Awake()
    {
        summonControls = new PlayerControls();
        
        exitClone = summonControls.SummonClone.ExitClone;
        exitClone.Enable();
        
        Player = GameObject.FindWithTag("Player");

        basicMovementPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<BasicMovement>();
        basicMovementClone = GameObject.FindGameObjectWithTag("Clone").GetComponent<BasicMovement>();
        
        cloneInteractions = FindObjectOfType<CloneInteractions>();
        summonClone = FindObjectOfType<SummonClone>();
        combatHandler = FindObjectOfType<CombatHandler>();
        limitedMovementCam = FindObjectOfType<LimitedMovementCam>();

        isOnPressurePlate = false;
        isOnLever = false;
        isRunning = false;
        isOnDoor = false;
        despawnClone = false;
        
        cloneActiveTimer = 30.0f;
        despawnCloneTimer = 1.25f;
        
        despawnTimerText = GameObject.FindGameObjectWithTag("DespawnTimer").GetComponent<TextMeshProUGUI>();
        despawnTimerText.text = "Clone Despawns In: " + Math.Round(despawnCloneTimer, 2);
        
        anim = GetComponent<Animator>();
        
        CheckForInteractables();
    }

    // Checks if the clone was spawned on or within an interactable collider or trigger.
    // If so, allow the clone to interact with that interactable.
    // Only accounts for pressure plates right now.
    private void CheckForInteractables()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f, pPlateLayer))
        {
            pressurePlate = hit.transform.GameObject().GetComponent<PressurePlate>();
            pressurePlate.CloneOnPlate();
            isOnPressurePlate = true;
        }
    }
    
    // Called between frames.
    void FixedUpdate()
    {
        // If the clone is summoned and the 'exit clone' button is pressed...
        if (exitClone.IsPressed() && summonClone.cloneSummoned)
        {
            // If the despawn timer is at or below 0, despawn the clone.
            if (despawnCloneTimer <= 0)
            {
                despawnClone = true;
            }
            // If the despawn timer is above 0,
            // subtract time from it and play the destruction sound.
            else
            {
                despawnCloneTimer = despawnCloneTimer - Time.deltaTime;
                despawnTimerText.text = "Clone Despawns In: " + Math.Round(despawnCloneTimer, 2);
                
                if (audioSource.clip != beginDestructSound)
                {
                    audioSource.clip = beginDestructSound;
                    audioSource.Play();
                }
            }
        }
        // Resets timer and sound values when the despawn button is released.
        else if (!exitClone.IsPressed() && despawnCloneTimer != 2)
        {
            audioSource.clip = null;
            
            despawnCloneTimer = 1.25f;
            despawnTimerText.text = "Clone Despawns In: " + Math.Round(despawnCloneTimer, 2);
        }

        // Check if the clone needs to be despawned or not.
        CheckCloneDespawn();
    }

    // Counts down clone timer, starting at 30 seconds.
    private void CloneCountdownTimer()
    {
        cloneActiveTimer -= Time.deltaTime;
        
        if (cloneActiveTimer <= 0)
        {
            despawnClone = true;
        }
    }

    // Despawns clone, giving control and camera back to the player, 
    // and resetting / destroying anything that needs to be reset or destroyed.
    // back to player if 'despawnClone' boolean value is true.
    // If clone is not yet despawned, countdown the clone timer until it does.
    private void CheckCloneDespawn()
    {
        if (despawnClone)
        {
           // Check to see if the clone was within range of any interactable 
           // objects before despawning.
            CheckInteractables();

            // Destroy any particle effects that were created by the clone 
            // and are not destroyed yet.
            if (this.GetComponent<AbilityPush>().chargeEffectDestroy != null)
            {
                Destroy(this.GetComponent<AbilityPush>().chargeEffectDestroy);
            }

            if (this.GetComponent<AbilityPush>().smallEffectDestroy != null)
            {
                Destroy(this.GetComponent<AbilityPush>().smallEffectDestroy);
            }
            if (this.GetComponent<AbilityPush>().largeEffectDestroy != null)
            {
                Destroy(this.GetComponent<AbilityPush>().largeEffectDestroy);
            }
            if (this.GetComponent<Grapple>().grappleEffectDestroy != null)
            {
                Destroy(this.GetComponent<Grapple>().grappleEffectDestroy);
            }

            // Destroy clone abilities.
            this.GetComponent<Grapple>().DestroyHook();
            this.GetComponent<AbilityPush>().DestroyShape();
            
            // Player clone destruction sound.
            AudioSource.PlayClipAtPoint(destructSound, transform.position);

            // Tell player that clone is despawned and allow player to move again.
            summonClone.cloneSummoned = false;
            basicMovementPlayer.canMove = true;
            
            // Enable player abilities.
            Player.GetComponent<Grapple>().enabled = true;
            Player.GetComponent<AbilityPush>().enabled = true;

            //activeTimerText.text = "";
            //combatHandler.healthText.text = "";
            despawnTimerText.text = "";

            // Give camera view and control back to player.
            limitedMovementCam.GetCurrentCameraData(basicMovementPlayer.curRoom);
            limitedMovementCam.SetCurrentPlayer(basicMovementPlayer.gameObject);

            // If the animator exists, tell it that the clone is despawned.
            if (anim != null)
            {
                anim.SetBool("isClone", false);
                animCover.SetBool("isClone", false);
            }

            // Finally, despawn the clone.
            Destroy(this.gameObject);
        }
        else
        {
            // Countdown the clone's active timer until it is despawned.
            CloneCountdownTimer();
        }
    }

    // Checks to see if the clone was within range of any interactable 
    // objects before despawning.
    // If the clone was within range of any interactable objects, tell those 
    // objects that the clone is no longer there before despawning.
    private void CheckInteractables()
    {
        if (isOnPressurePlate)
        {
            pressurePlate.isClone = false;
            
            // If the player is also on the pressure plate, 
            // do not deactivate it.
            if (!pressurePlate.isPlayer)
            {
                pressurePlate.AppearWall();
            }
        }

        if (isOnLever)
        {
            lever.isClone = false;

            // If the player is also at the lever, 
            // do not deactivate it's text.
            if (!lever.isPlayer)
            {
                //lever.activateText.enabled = false;
            }
        }

        if (isOnDoor && door.activateText != null)
        {
            door.isClone = false;

            // If the player is also at the door, 
            // do not deactivate it's text.
            if (!door.isPlayer)
            {
                door.activateText.enabled = false;
            }
        }
    }

    // Disable input action map controls.
    private void OnDisable()
    {
        exitClone.Disable();
    }

    // Determines whether clone is interacting with any
    // interactable objects.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PressurePlate"))
        {
            pressurePlate = other.GetComponent<PressurePlate>();

            isOnPressurePlate = true;
        }
        else if (other.CompareTag("Lever"))
        {
            lever = other.GetComponent<Lever>();

            isOnLever = true;
        }
        else if (other.CompareTag("Door"))
        {
            door = other.GetComponent<Door>();
            
            isOnDoor = true;
        }
    }

    // Determines whether clone is exiting any interactable objects.
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PressurePlate"))
        {
            isOnPressurePlate = false;
            isOnLever = false;
        }
        else if (other.CompareTag("Lever"))
        {
            isOnLever = false;
        }
        else if (other.CompareTag("Door"))
        {
            isOnDoor = false;
        }
    }
}
