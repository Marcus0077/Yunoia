using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class ExitClone : MonoBehaviour
{
    // Audio variables.
    public AudioSource audioSource;
    public AudioClip beginDestructSound;
    public AudioClip destructSound;
    
    // Temporary Despawn Timer UI.
    public float despawnTimerText;
    public Image cloneIcon;
    
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
    public PlayerControls summonControls;
    public InputAction exitClone;

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
        Player = GameObject.FindWithTag("Player");
        exitClone = Player.GetComponent<SummonClone>().exitClone;
        exitClone.Enable();
        
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
        

        cloneIcon = GameObject.FindGameObjectWithTag("DespawnTimer").GetComponent<Image>();
        if(cloneIcon != null)
            cloneIcon.fillAmount = 0;

        anim = GetComponent<Animator>();
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
                if(cloneIcon == null)
                    cloneIcon = GameObject.FindGameObjectWithTag("DespawnTimer").GetComponent<Image>();
                despawnCloneTimer = despawnCloneTimer - Time.deltaTime;
                //cloneIcon.fillAmount = 1;
                
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
            if (cloneIcon == null)
                cloneIcon = GameObject.FindGameObjectWithTag("DespawnTimer").GetComponent<Image>();
            //cloneIcon.fillAmount = 0;
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
            //Set all Faceless AI that are free to wander.
            foreach (var Faceless in GameObject.FindObjectsOfType<AiMovement>())
            {
                if (Faceless.canAiMove && !Faceless.isStoppedByCrystal && !Faceless.isFollowingCrystal)
                {

                    Faceless.wanderCoroutine = Faceless.Wander();
                    StartCoroutine(Faceless.wanderCoroutine);
                    Faceless.isWanderRunning = true;
                }
            }
            
           // Check to see if the clone was within range of any interactable 
           // objects before despawning.
            CheckInteractables();

            // Destroy any particle effects that were created by the clone 
            // and are not destroyed yet.
            DestroyParticles();

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
            if (cloneIcon == null)
                cloneIcon = GameObject.FindGameObjectWithTag("DespawnTimer").GetComponent<Image>();
            //cloneIcon.fillAmount = 0;

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

    // Destroy any particle effects that were created by the clone 
    // and are not destroyed yet.
    private void DestroyParticles()
    {
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

        if (GetComponent<BasicMovement>().inAngerRoom)
        { 
            if (GetComponent<BasicMovement>().curAngerRoomPartialTrigger != null)
            {
                GetComponent<BasicMovement>().curAngerRoomPartialTrigger.isClone = false;
                
                if (!GetComponent<BasicMovement>().curAngerRoomPartialTrigger.isPlayer)
                {
                    GetComponent<BasicMovement>().curAngerRoomPartialTrigger.PartialViewWalls();
                }
            }
            
            if (GetComponent<BasicMovement>().curAngerRoomFullTrigger != null)
            {
                GetComponent<BasicMovement>().curAngerRoomFullTrigger.isClone = false;

                if (!GetComponent<BasicMovement>().curAngerRoomFullTrigger.isPlayer)
                {
                    Debug.Log("attempts to make chair viewable");
                    
                    GetComponent<BasicMovement>().curAngerRoomFullTrigger.FullyViewWalls();
                }
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
