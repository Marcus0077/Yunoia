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
    
    // Script references
    private BasicMovement basicMovementPlayer;
    private BasicMovement basicMovementClone;
    private SmoothCameraFollow smoothCameraFollow;
    private SummonClone summonClone;
    private CombatHandler combatHandler;
    private CloneInteractions cloneInteractions;
    
    // Player reference
    public GameObject Player;

    // Input variables
    PlayerControls summonControls;
    private InputAction exitClone;

    // Despawn clone conditional variable
    public bool despawnClone;
    
    // Timer variables
    private float cloneActiveTimer;
    private bool isRunning;

    // Get references and initialize variables when clone is instantiated.
    void Awake()
    {
        summonControls = new PlayerControls();
        
        Player = GameObject.FindWithTag("Player");

        basicMovementPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<BasicMovement>();
        basicMovementClone = GameObject.FindGameObjectWithTag("Clone").GetComponent<BasicMovement>();
        cloneInteractions = FindObjectOfType<CloneInteractions>();
        smoothCameraFollow = FindObjectOfType<SmoothCameraFollow>();
        summonClone = FindObjectOfType<SummonClone>();
        combatHandler = FindObjectOfType<CombatHandler>();
            
        isRunning = false;
        despawnClone = false;
        cloneActiveTimer = 30.0f;
        
        activeTimerText = GameObject.FindGameObjectWithTag("Active Timer").GetComponent<TextMeshProUGUI>();
        activeTimerText.color = Color.white;
    }
    
    // Called between frames.
    void FixedUpdate()
    {
        CheckCloneDespawn();
    }

    // Counts down clone timer, starting at 30 seconds.
    // At 5 seconds, makes the clone blink on and off and turn the 
    // timer text red.
    private void CloneCountdownTimer()
    {
        cloneActiveTimer -= Time.deltaTime;
        activeTimerText.text = "Clone Despawns In: " + Math.Round(cloneActiveTimer, 2); 
        
        if (cloneActiveTimer <= 0)
        {
            despawnClone = true;
        }
        else if (cloneActiveTimer < 5.01)
        {
            activeTimerText.color = Color.red;

            if (!isRunning)
            {
                StartCoroutine(Blink());
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
            summonClone.cloneSummoned = false;
            basicMovementPlayer.canMove = true;
            
            Player.GetComponent<Grapple>().enabled = true;
            Player.GetComponent<AbilityPush>().enabled = true;

            if (cloneInteractions.isOnTrigger2)
            {
                cloneInteractions.Blocker2.GetComponent<MeshRenderer>().enabled = true;
                cloneInteractions.Blocker2.GetComponent<Collider>().enabled = true;
            }

            activeTimerText.text = "";
            combatHandler.healthText.text = "";

            cloneInteractions.Blocker3.transform.position = cloneInteractions.blocker3InPos;

            smoothCameraFollow.target = basicMovementPlayer.playerRB.transform;

            Destroy(this.gameObject);
        }
        else
        {
            CloneCountdownTimer();
        }
    }

    // Called each frame.
    private void Update()
    {
        if (exitClone.WasPressedThisFrame() && summonClone.cloneSummoned)
        {
            despawnClone = true;
        }
    }

    // Flashes the clone's renderer on and off.
    IEnumerator Blink()
    {
        isRunning = true;
        
        this.gameObject.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        this.gameObject.GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(0.4f);
        
        isRunning = false;
    }
    
    // Enable input action map controls.
    private void OnEnable()
    {
        exitClone = summonControls.SummonClone.ExitClone;
        exitClone.Enable();
    }

    // Disable input action map controls.
    private void OnDisable()
    {
        exitClone.Disable();
    }

    // Destroys clone if it runs into a "Bad Wall"
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BadWall")
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), this.GetComponent<Collider>());
            despawnClone = true;
        }
    }
}
