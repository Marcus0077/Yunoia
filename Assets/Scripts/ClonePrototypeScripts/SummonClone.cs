using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class SummonClone : MonoBehaviour
{
    // Script references
    private BasicMovementPlayer basicMovementPlayer;
    private BasicMovementClone basicMovementClone;

    // Input variables
    PlayerControls summonControls;
    private InputAction summonAClone;

    // Clone Summon variables
    public GameObject ClonePrefab;
    public bool cloneSummoned;

    // Get references and initialize variables when player spawns.
    void Awake()
    {
        basicMovementPlayer = FindObjectOfType<BasicMovementPlayer>();
        basicMovementClone = FindObjectOfType<BasicMovementClone>();

        summonControls = new PlayerControls();
        
        cloneSummoned = false;
    }

    void Update()
    {
        if (!cloneSummoned && summonAClone.WasPressedThisFrame())
        {
            SummonAClone();
        }
    }
    
    // Summon a clone at a specified location, freeze player, and deactivate summon control.
    void SummonAClone()
    {
        basicMovementPlayer.canMove = false;
        this.GetComponent<Grapple>().enabled = false;
        this.GetComponent<AbilityPush>().enabled = false;
        
        cloneSummoned = true;
        
        Instantiate(ClonePrefab, basicMovementPlayer.playerRB.position + Vector3.right, Quaternion.identity);
    }

    // Enable input action map controls.
    private void OnEnable()
    {
        summonAClone = summonControls.SummonClone.SummonAClone;
        summonAClone.Enable();
    }

    // Disable input action map controls.
    private void OnDisable()
    {
        summonAClone.Disable();
    }
}
