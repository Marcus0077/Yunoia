using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;

public class SummonClone : MonoBehaviour
{
    // Script references
    private BasicMovement basicMovementPlayer;

    // Input variables
    PlayerControls summonControls;
    private InputAction summonAClone;

    // Clone Summon variables
    public GameObject ClonePrefab;
    public bool cloneSummoned;

    public LayerMask ground;
    public LayerMask wall;

    // Get references and initialize variables when player spawns.
    void Awake()
    {
        basicMovementPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<BasicMovement>();

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
        Vector3 rightOfPlayer = new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z - 1f);
        
        RaycastHit hit;

        Debug.DrawRay(transform.position, Vector3.back * 1.25f, Color.green, 2f);
        Debug.DrawRay(rightOfPlayer, Vector3.down * 1f, Color.green, 2f);
        
        
        if (Physics.Raycast(transform.position, Vector3.back, out hit, 1.25f, wall) || !Physics.Raycast(rightOfPlayer, Vector3.down, out hit, 1.25f, ground))
        {
            Debug.Log("Did Hit");
        }
        else
        {
            basicMovementPlayer.canMove = false;
            this.GetComponent<Grapple>().enabled = false;
            this.GetComponent<AbilityPush>().enabled = false;
        
            cloneSummoned = true;
        
            Instantiate(ClonePrefab, basicMovementPlayer.playerRB.position + Vector3.back, Quaternion.identity);
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
