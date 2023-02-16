using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ExitClone : MonoBehaviour
{
    // UI Clone Active Timer
    public TextMeshProUGUI activeTimerText;
    
    // Temporary Despawn Timer UI
    public TextMeshProUGUI despawnTimerText;
    
    //UI Experiment
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
    
    // Player reference
    public GameObject Player;

    // Input variables
    PlayerControls summonControls;
    private InputAction exitClone;

    // Despawn clone variables
    public bool despawnClone;
    public float despawnCloneTimer;
    
    // Timer variables
    public float cloneActiveTimer;
    private bool isRunning;

    // Interactable trigger variables.
    private bool isOnPressurePlate;
    private bool isOnLever;
    private bool isOnDoor;

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
        despawnCloneTimer = 2.0f;
        
        //activeTimerText = GameObject.FindGameObjectWithTag("Active Timer").GetComponent<TextMeshProUGUI>();
        //activeTimerText.color = Color.white;
        
        despawnTimerText = GameObject.FindGameObjectWithTag("DespawnTimer").GetComponent<TextMeshProUGUI>();
        despawnTimerText.text = "Clone Despawns In: " + Math.Round(despawnCloneTimer, 2);
        
        anim = GetComponent<Animator>();
    }

    // Called each frame.
    private void Update()
    {
        
    }
    
    // Called between frames.
    void FixedUpdate()
    {
        if (exitClone.IsPressed() && summonClone.cloneSummoned)
        {
            if (despawnCloneTimer <= 0)
            {
                despawnClone = true;
            }
            else
            {
                despawnCloneTimer = despawnCloneTimer - Time.deltaTime;
                despawnTimerText.text = "Clone Despawns In: " + Math.Round(despawnCloneTimer, 2);
            }
        }
        else if (!exitClone.IsPressed() && despawnCloneTimer != 2)
        {
            despawnCloneTimer = 2.0f;
            despawnTimerText.text = "Clone Despawns In: " + Math.Round(despawnCloneTimer, 2);
        }

        CheckCloneDespawn();
    }

    // Counts down clone timer, starting at 30 seconds.
    // At 5 seconds, makes the clone blink on and off and turn the 
    // timer text red.
    private void CloneCountdownTimer()
    {
        cloneActiveTimer -= Time.deltaTime;
        //activeTimerText.text = "Clone Despawns In: " + Math.Round(cloneActiveTimer, 2); 
        
        if (cloneActiveTimer <= 0)
        {
            despawnClone = true;
        }
        else if (cloneActiveTimer < 5.01)
        {
            //activeTimerText.color = Color.red;

            if (!isRunning)
            {
                //StartCoroutine(Blink());
            }
        }
    }

    // Despawns clone, resets triggers, takes away clone UI text, and gives control
    // back to player if 'despawnClone' boolean value is true.
    // If clone is not yet despawned, countdown the clone timer until it does.
    private void CheckCloneDespawn()
    {
        if (despawnClone)
        {
            CheckInteractables();

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

            this.GetComponent<Grapple>().DestroyHook();
            this.GetComponent<AbilityPush>().DestroyShape();
            
            summonClone.cloneSummoned = false;
            basicMovementPlayer.canMove = true;
            
            Player.GetComponent<Grapple>().enabled = true;
            Player.GetComponent<AbilityPush>().enabled = true;

            //activeTimerText.text = "";
            //combatHandler.healthText.text = "";
            despawnTimerText.text = "";

            limitedMovementCam.GetCurrentCameraData(basicMovementPlayer.curRoom);
            limitedMovementCam.SetCurrentPlayer(basicMovementPlayer.gameObject);

            if (anim != null)
            {
                anim.SetBool("isClone", false);
                animCover.SetBool("isClone", false);
            }

            Destroy(this.gameObject);
        }
        else
        {
            CloneCountdownTimer();
        }
    }

    private void CheckInteractables()
    {
        if (isOnPressurePlate)
        {
            pressurePlate.AppearWall();
            pressurePlate.isClone = false;
        }

        if (isOnLever)
        {
            lever.activateText.enabled = false;
            lever.isClone = false;
        }

        if (isOnDoor && door.activateText != null)
        {
            door.activateText.enabled = false;
            door.isClone = false;
        }
    }

    // Flashes the clone's renderer on and off. (currently unused)
    IEnumerator Blink()
    {
        isRunning = true;
        
        this.gameObject.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        this.gameObject.GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(0.4f);
        
        isRunning = false;
    }

    // Disable input action map controls.
    private void OnDisable()
    {
        exitClone.Disable();
    }

    // Determines whether clone is on a pressure plate or in a lever trigger.
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

            if (lever.isClone)
            {
                isOnLever = true;
            }
        }
        else if (other.CompareTag("Door"))
        {
            door = other.GetComponent<Door>();

            if (door.isClone)
            {
                isOnDoor = true;
            }
        }
    }

    // Determines whether clone is exiting a pressure plate or a lever trigger.
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
