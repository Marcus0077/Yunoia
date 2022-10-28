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
    private BasicMovementPlayer basicMovementPlayer;
    private BasicMovementClone basicMovementClone;
    private SmoothCameraFollow smoothCameraFollow;
    private SummonClone summonClone;
    private CombatHandler combatHandler;
    
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

        basicMovementPlayer = FindObjectOfType<BasicMovementPlayer>();
        basicMovementClone = FindObjectOfType<BasicMovementClone>();
        smoothCameraFollow = FindObjectOfType<SmoothCameraFollow>();
        summonClone = FindObjectOfType<SummonClone>();
        combatHandler = FindObjectOfType<CombatHandler>();
            
        isRunning = false;
        despawnClone = false;
        cloneActiveTimer = 30.0f;
        
        activeTimerText = GameObject.FindGameObjectWithTag("Active Timer").GetComponent<TextMeshProUGUI>();
        activeTimerText.color = Color.white;
    }
    
    void FixedUpdate()
    {
        // Allow player to move, reset summonClone script boolean values, 
        // reset prototype text, and destroy clone.
        if (exitClone.IsPressed() || despawnClone)
        {
            summonClone.cloneSummoned = false;
            basicMovementPlayer.playerCanMove = true;
            
            Player.GetComponent<Grapple>().enabled = true;
            Player.GetComponent<AbilityPush>().enabled = true;

            if (basicMovementClone.isOnTrigger2)
            {
                basicMovementClone.Blocker2.GetComponent<MeshRenderer>().enabled = true;
                basicMovementClone.Blocker2.GetComponent<Collider>().enabled = true;
            }

            activeTimerText.text = "";
            combatHandler.healthText.text = "";

            basicMovementClone.Blocker3.transform.position = basicMovementClone.blocker3InPos;

            smoothCameraFollow.target = basicMovementPlayer.playerRB.transform;

            Destroy(this.gameObject);
        }
        // If clone is still active, count down.
        else
        {
            cloneActiveTimer -= Time.deltaTime;
            activeTimerText.text = "Clone Despawns In: " + Math.Round(cloneActiveTimer, 2); 
        }

        // If clone is not active, despawn it.
        if (cloneActiveTimer <= 0)
        {
            despawnClone = true;
        }
        // If 5 seconds are left, turn active time red and make clone blink.
        else if (cloneActiveTimer < 5.01)
        {
            activeTimerText.color = Color.red;

            if (!isRunning)
            {
                StartCoroutine(Blink());
            }
        }
    }
    
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

    private void OnCollisionEnter(Collision collision)
    {
        // Allow clone to pass through special walls.
        if (collision.gameObject.tag == "BadWall")
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), this.GetComponent<Collider>());
            despawnClone = true;
        }
    }
}
