using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ExitClone : MonoBehaviour
{
    // UI Active Timer
    public TextMeshProUGUI activeTimerText;
    
    // Script references
    private BasicMovementPlayer basicMovementPlayer;
    private BasicMovementClone basicMovementClone;
    private SummonClone summonClone;
    
    // Input variables
    PlayerControls summonControls;
    private InputAction exitClone;

    // Despawn clone conditional variable
    private bool despawnClone;
    
    // Timer variables
    private float cloneActiveTimer;
    private bool isRunning;

    // Get references and initialize variables when clone is instantiated.
    void Awake()
    {
        summonControls = new PlayerControls();
        
        summonClone = FindObjectOfType<SummonClone>();
        
        basicMovementPlayer = FindObjectOfType<BasicMovementPlayer>();
        basicMovementClone = FindObjectOfType<BasicMovementClone>();

        isRunning = false;
        despawnClone = false;
        cloneActiveTimer = 10.0f;
        
        activeTimerText = GameObject.FindGameObjectWithTag("Active Timer").GetComponent<TextMeshProUGUI>();
        activeTimerText.color = Color.white;
        activeTimerText.text = "Active Timer: ";
    }
    
    void FixedUpdate()
    {
        // Allow player to move, reset summonClone script boolean values, 
        // reset prototype text, and destroy clone.
        if (exitClone.IsPressed() || despawnClone)
        {
            basicMovementPlayer.canMove = true;
            
            summonClone.canInitiateCustom = true;
            summonClone.canSummonPreset = true;
            summonClone.cloneSummoned = false;
            summonClone.presetMode = false;
            summonClone.customMode = false;

            basicMovementClone.inControlText.text = "";
            basicMovementClone.cooldownTimerText.text = "";
            activeTimerText.text = "";
            
            summonClone.prototypeModeText.text = "Please select a prototype mode...";
            summonClone.prototypeModeText.color = Color.red;
            
            Destroy(this.gameObject);
        }
        // If clone is still active, count down.
        else
        {
            cloneActiveTimer -= Time.deltaTime;
            activeTimerText.text = "Active Timer: " + Math.Round(cloneActiveTimer, 2); 
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
