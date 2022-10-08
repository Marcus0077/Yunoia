using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExitClone : MonoBehaviour
{
    // Script references
    private BasicMovementPlayer basicMovementPlayer;
    private BasicMovementClone basicMovementClone;
    private SummonClone summonClone;
    
    // Input variables
    PlayerControls summonControls;
    private InputAction exitClone;
    
    // Get references and initialize variables when clone is instantiated.
    void Awake()
    {
        summonControls = new PlayerControls();
        
        summonClone = FindObjectOfType<SummonClone>();
        
        basicMovementPlayer = FindObjectOfType<BasicMovementPlayer>();
        basicMovementClone = FindObjectOfType<BasicMovementClone>();
    }
    
    void FixedUpdate()
    {
        // Allow player to move, reset summonClone script boolean values, 
        // reset prototype text, and destroy clone.
        if (exitClone.IsPressed())
        {
            basicMovementPlayer.canMove = true;
            
            summonClone.canInitiateCustom = true;
            summonClone.canSummonPreset = true;
            summonClone.cloneSummoned = false;
            summonClone.presetMode = false;
            summonClone.customMode = false;

            basicMovementClone.timerText.text = "";
            
            summonClone.prototypeModeText.text = "Please select a prototype mode...";
            summonClone.prototypeModeText.color = Color.red;
            
            Destroy(this.gameObject);
        }
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
}
