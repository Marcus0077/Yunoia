using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class SummonClone : MonoBehaviour
{
    // Prototype mode selection text
    public TextMeshProUGUI prototypeModeText;
    
    // Script references
    private BasicMovementPlayer basicMovementPlayer;
    
    // Input variables
    PlayerControls summonControls;
    private InputAction initiateCustomSummon;
    private InputAction exitCustomSummon;
    private InputAction customSummonMouse;
    private InputAction customSummonGamepad;
    private InputAction summonAClone;

    // Universal Clone Summon variables
    public GameObject ClonePrefab;
    
    public bool cloneSummoned;
    
    // Custom Clone Summon variables
    public Transform customSummonCursor; 
    
    public LayerMask whatIsGround;
    
    private Vector3 mousePosition; 
    private Vector3 mousePositionCheck; 
    private Vector3 gamepadPosition;
    private Vector3 deltaAccumulator; 
    
    public bool customMode;
    public bool canInitiateCustom;
    private bool canSummonCustom;
    
    // Preset Clone Summon variables
    public Transform[] presetSummonTriggers = new Transform[3]; 
    public Transform[] presetSummonSpawns = new Transform[3];

    public bool presetMode;
    public bool canSummonPreset;

    // Get references and initialize variables when player spawns.
    void Awake()
    {
        basicMovementPlayer = FindObjectOfType<BasicMovementPlayer>();
        summonControls = new PlayerControls();
        
        customSummonCursor.gameObject.SetActive(false);
        
        prototypeModeText.text = "Please select a prototype mode...";
        prototypeModeText.color = Color.red;

        canInitiateCustom = true;
        canSummonCustom = false;
        customMode = false;
        presetMode = false;
        canSummonPreset = true;
        cloneSummoned = false;
        
        deltaAccumulator = basicMovementPlayer.playerRB.transform.position;
        
        InstantiateSummonTriggers(false);
        InstantiateSummonSpawn(-1);
    }
    
    // Fun GUI buttons
    private void OnGUI()
    {
        // When this button is pressed, the 'Custom Spawn Location' Prototype Mode is selected
        // and 'Preset Spawn Location' is disabled.
        if (GUI.Button(new Rect(20, 90, 300, 50), "Clone Prototype: Custom Spawn Location") && !cloneSummoned)
        {
            prototypeModeText.text = "Prototype Mode: Custom Spawn Location";
            prototypeModeText.color = Color.green;

            customMode = true;
            presetMode = false;
            
            InstantiateSummonTriggers(false);
            InstantiateSummonSpawn(-1);
        }
        
        // When this button is pressed, the 'Preset Spawn Location' Prototype Mode is selected
        // and 'Custom Spawn Location' is disabled.
        if (GUI.Button(new Rect(20, 10, 300, 50), "Clone Prototype: Preset Spawn Location") && !cloneSummoned)
        {
            prototypeModeText.text = "Prototype Mode: Preset Spawn Location";
            prototypeModeText.color = Color.green;
            
            presetMode = true;
            customMode = false;
            
            InstantiateSummonTriggers(true);

        }
    }
    
    void FixedUpdate()
    {
        // If Custom Mode is selected, we can either initiate a custom
        // summon location or summon at that chosen location (if already chosen).
        if (customMode)
        {
            if (canInitiateCustom)
            {
                InitiateCustomSummon();
            }

            if (canSummonCustom)
            {
                CustomSummonLocation();
            }
        }
        
        // If Preset Mode is selected, we check to see which preset
        // summon location that the player has activated.
        if (presetMode && canSummonPreset)
        {
            PresetSummonLocation();
        }
    }

    // If initiateCustomSummon button is pressed, lock player movement 
    // and allow for custom summon location selection.
    void InitiateCustomSummon()
    {
        if (initiateCustomSummon.IsPressed())
        {
            canInitiateCustom = false;
            canSummonCustom = true;
            basicMovementPlayer.canMove = false;
        }
    }

    // Activate cursor that can be cast along any ground-layer,
    // using mouse or gamepad position, to choose a custom summon location.
    void CustomSummonLocation()
    {
        customSummonCursor.gameObject.SetActive(true);

        MouseLocation();
        GamepadLocation();
        
        if (summonAClone.IsPressed())
        {
            customSummonCursor.gameObject.SetActive(false);
            
            SummonAClone(customSummonCursor.position);
        }

        if (exitCustomSummon.IsPressed())
        { 
            customSummonCursor.gameObject.SetActive(false);
            
            canInitiateCustom = true;
            canSummonCustom = false;
            basicMovementPlayer.canMove = true;
            
        }
    }

    // Finds mouse location for custom summon cursor.
    void MouseLocation()
    {
        mousePosition = new Vector3(customSummonMouse.ReadValue<Vector2>().x, customSummonMouse.ReadValue<Vector2>().y, 0f);
        if (mousePositionCheck.Equals(mousePosition))
        {
            return;
        }
        mousePositionCheck = mousePosition;

        deltaAccumulator = Vector3.zero;
        
        RaycastHit hit;
        Ray cameraRay = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(cameraRay, out hit, Mathf.Infinity, whatIsGround))
        {
            if (customSummonCursor.position != hit.point)
            {
                customSummonCursor.position = hit.point;
            }
        }
    }
    
    // Finds gamepad location (relative to mouse location) for custom summon cursor.
    void GamepadLocation()
    {
        gamepadPosition = new Vector3(customSummonGamepad.ReadValue<Vector2>().x, customSummonGamepad.ReadValue<Vector2>().y, 0f);
        
        deltaAccumulator += gamepadPosition;

        RaycastHit hit;
        Ray cameraRay = Camera.main.ScreenPointToRay(mousePosition + deltaAccumulator);
        if (Physics.Raycast(cameraRay, out hit, Mathf.Infinity, whatIsGround))
        {
            if (customSummonCursor.position != hit.point)
            {
                customSummonCursor.position = hit.point;
            }
        }
    }
    
    // If summonAClone button is pressed, check which summon trigger is activated
    // and summon a clone at that trigger's corresponding summon spawn location.
    void PresetSummonLocation()
    {
        if (summonAClone.IsPressed())
        {
            for (int i = 0; i < 3; i++)
            {
                if (presetSummonSpawns[i].GetComponent<MeshRenderer>().enabled == true)
                {
                    presetSummonSpawns[i].GetComponent<MeshRenderer>().enabled = false;
                
                    InstantiateSummonTriggers(false);

                    SummonAClone(presetSummonSpawns[i].position);
                }
            }
        }
    }

    // Summon a clone at a specified location, freeze player, and deactivate summon control.
    void SummonAClone(Vector3 spawnPos)
    {
        Instantiate(ClonePrefab, spawnPos + Vector3.up, Quaternion.identity);
        
        basicMovementPlayer.canMove = false;
        
        canInitiateCustom = false;
        canSummonCustom = false;
        canSummonPreset = false;
        cloneSummoned = true;
    }

    // Enable or disable summon trigger renderers.
    private void InstantiateSummonTriggers(bool instantiate)
    {
        if (instantiate)
        {
            for (int i = 0; i < 3; i++)
            {
                presetSummonTriggers[i].GetComponent<MeshRenderer>().enabled = true;
            }
        }
        
        else if (!instantiate)
        {
            for (int i = 0; i < 3; i++)
            {
                presetSummonTriggers[i].GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
    
    // Enable summon spawn renderer depending on which summon trigger is activated.
    private void InstantiateSummonSpawn(int summonNumber)
    {
        for (int i = 0; i < 3; i++)
        {
            if (summonNumber == i)
            {
                presetSummonSpawns[i].GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {
                presetSummonSpawns[i].GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Check which summon trigger is activated.
        if (presetMode && !cloneSummoned)
        {
            for (int i = 1; i < 4; i++)
            {
                if (other.gameObject.CompareTag("SpawnTrigger" + i))
                {
                    InstantiateSummonSpawn(i-1);
                }
            }
        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        if (presetMode && !cloneSummoned)
        {
            for (int i = 1; i < 4; i++)
            {
                if (other.gameObject.CompareTag("SpawnTrigger" + i))
                {
                    InstantiateSummonSpawn(-1);
                }
            }
        }
    }
    

    // Enable input action map controls.
    private void OnEnable()
    {
        initiateCustomSummon = summonControls.SummonClone.InitiateCustomSummon;
        initiateCustomSummon.Enable();

        customSummonMouse = summonControls.SummonClone.CustomSummonMouse;
        customSummonMouse.Enable();
        customSummonGamepad = summonControls.SummonClone.CustomSummonGamepad;
        customSummonGamepad.Enable();

        exitCustomSummon = summonControls.SummonClone.ExitCustomSummon;
        exitCustomSummon.Enable();

        summonAClone = summonControls.SummonClone.SummonAClone;
        summonAClone.Enable();
    }

    // Disable input action map controls.
    private void OnDisable()
    {
        initiateCustomSummon.Disable();
        customSummonMouse.Disable();
        customSummonGamepad.Disable();
        exitCustomSummon.Disable();
        summonAClone.Disable();
    }
}
