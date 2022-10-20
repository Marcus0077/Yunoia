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
    public bool canSummon;
    
    public TextMeshProUGUI guidanceText;
    public TextMeshProUGUI cloneVersionText;


    // Get references and initialize variables when player spawns.
    void Awake()
    {
        guidanceText = GameObject.FindGameObjectWithTag("GuidanceText").GetComponent<TextMeshProUGUI>();
        guidanceText.text = "Welcome to the Clone Summon Prototype! Step on the yellow summoning plate to begin!";
        
        cloneVersionText = GameObject.FindGameObjectWithTag("CloneVersionText").GetComponent<TextMeshProUGUI>();
        cloneVersionText.text = "";
        
        basicMovementPlayer = FindObjectOfType<BasicMovementPlayer>();
        basicMovementClone = FindObjectOfType<BasicMovementClone>();

        summonControls = new PlayerControls();
        
        cloneSummoned = false;
    }

    void Update()
    {
        if (canSummon && !cloneSummoned && summonAClone.WasPressedThisFrame())
        {
            SummonAClone();
        }
    }
    
    // Summon a clone at a specified location, freeze player, and deactivate summon control.
    void SummonAClone()
    {
        basicMovementPlayer.canMove = false;
        
        cloneSummoned = true;
        
        Instantiate(ClonePrefab, basicMovementPlayer.playerRB.position + Vector3.right, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SpawnTrigger")  && !cloneSummoned)
        {
            canSummon = true;
            
            guidanceText.text = "Left-Click (mouse) or press 'X' (controller) to summon a clone!";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("SpawnTrigger") && !cloneSummoned)
        {
            canSummon = false;
            
            guidanceText.text = "Welcome to the Clone Summon Prototype! Step on the yellow summoning plate to begin!";
        }
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
