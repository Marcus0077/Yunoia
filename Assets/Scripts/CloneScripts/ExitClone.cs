using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ExitClone : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip beginDestructSound;
    public AudioClip destructSound;

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
    
    // Layer Mask Variables
    public LayerMask pPlateLayer;

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
        
        //activeTimerText = GameObject.FindGameObjectWithTag("Active Timer").GetComponent<TextMeshProUGUI>();
        //activeTimerText.color = Color.white;
        
        despawnTimerText = GameObject.FindGameObjectWithTag("DespawnTimer").GetComponent<TextMeshProUGUI>();
        despawnTimerText.text = "Clone Despawns In: " + Math.Round(despawnCloneTimer, 2);
        
        anim = GetComponent<Animator>();
        
        CheckForInteractables();
    }

    private void CheckForInteractables()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f, pPlateLayer))
        {
            Debug.Log("on pplate");
            
            pressurePlate = hit.transform.GameObject().GetComponent<PressurePlate>();
            pressurePlate.CloneOnPlate();
            isOnPressurePlate = true;
        }
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
                if (audioSource.clip != beginDestructSound)
                {
                    audioSource.clip = beginDestructSound;
                    audioSource.Play();
                }
            }
        }
        else if (!exitClone.IsPressed() && despawnCloneTimer != 2)
        {
            audioSource.clip = null;
            despawnCloneTimer = 1.25f;
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
            
            AudioSource.PlayClipAtPoint(destructSound,transform.position);

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
            pressurePlate.isClone = false;
            
            if (!pressurePlate.isPlayer)
            {
                pressurePlate.AppearWall();
            }
        }

        if (isOnLever)
        {
            lever.isClone = false;

            if (!lever.isPlayer)
            {
                //lever.activateText.enabled = false;
            }
        }

        if (isOnDoor && door.activateText != null)
        {
            door.isClone = false;

            if (!door.isPlayer)
            {
                door.activateText.enabled = false;
            }
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

            isOnLever = true;
        }
        else if (other.CompareTag("Door"))
        {
            door = other.GetComponent<Door>();
            
            isOnDoor = true;
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
