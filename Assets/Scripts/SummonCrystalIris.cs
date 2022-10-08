using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class SummonCrystalIris : MonoBehaviour
{
    PlayerControls summonControls;
    private InputAction initiateSummon;
    private InputAction summonLocationGamepad;
    private InputAction summonLocationMouse;
    private InputAction summonClone;
    private InputAction exitSummonLocation;

    public Transform summonCursor;

    public bool canInitiate;
    private bool canSummon;

    public LayerMask whatIsGround;

    public GameObject CrystalIrisPrefab;

    private BasicMovementIris basicMovementIris;

    private Vector3 deltaAccumulator;

    private Vector3 mPosCheck;

    private Vector3 mPos;
    
    public TextMeshProUGUI prototypeModeText;
    public bool customMode;
    public bool presetMode;
    
    public Transform[] spawnTriggers = new Transform[3];
    public Transform[] cloneSpawns = new Transform[3];

    public bool canSpawn;
    public bool inCrystal;

    private int[] whatSpawnToSummon = new int[3];
    
    void Awake()
    {
        summonControls = new PlayerControls();
        canInitiate = true;
        canSummon = false;
        summonCursor.gameObject.SetActive(false);
        basicMovementIris = FindObjectOfType<BasicMovementIris>();
        deltaAccumulator = basicMovementIris.irisRB.transform.position;

        prototypeModeText.text = "Please select a prototype mode...";
        prototypeModeText.color = Color.red;
        customMode = false;
        presetMode = false;
        
        InstantiateSpawnTriggers(false);
        InstantiateCloneSpawn(-1);

        canSpawn = true;
        inCrystal = false;
    }
    
    void FixedUpdate()
    {
        InitiateSummon();
        
        if (canSummon)
        {
            SummonLocation();
        }

        if (canSpawn)
        {
            SpawnLocation();
        }
    }

    void InitiateSummon()
    {
        if (initiateSummon.IsPressed() && canInitiate && customMode)
        {
            canInitiate = false;
            canSummon = true;
            basicMovementIris.canMove = false;
        }
    }

    void SummonLocation()
    {
        if (customMode)
        {
            summonCursor.gameObject.SetActive(true);

            GamepadLocation();
            MouseLocation();

            if (summonClone.IsPressed())
            {
                SummonClone(summonCursor.position);
                canSummon = false;
                summonCursor.gameObject.SetActive(false);
            }

            if (exitSummonLocation.IsPressed())
            {
                canInitiate = true;
                canSummon = false;
                basicMovementIris.canMove = true;
                summonCursor.gameObject.SetActive(false);
            }
        }
    }

    void SpawnLocation()
    {
        if (presetMode && initiateSummon.IsPressed())
        {
            CheckSpawnsThenSummon();
            Debug.Log("works");
        }
    }

    void MouseLocation()
    {
        mPos = new Vector3(summonLocationMouse.ReadValue<Vector2>().x, summonLocationMouse.ReadValue<Vector2>().y, 0f);
        if (mPosCheck.Equals(mPos))
        {
            return;
        }
        mPosCheck = mPos;

        deltaAccumulator = Vector3.zero;
        
        RaycastHit hit;
        Ray cameraRay = Camera.main.ScreenPointToRay(mPos);
        if (Physics.Raycast(cameraRay, out hit, Mathf.Infinity, whatIsGround))
        {
            if (summonCursor.position != hit.point)
            {
                summonCursor.position = hit.point;
            }
        }
    }
    void GamepadLocation()
    {
        Vector3 gPos = new Vector3(summonLocationGamepad.ReadValue<Vector2>().x, summonLocationGamepad.ReadValue<Vector2>().y, 0f);
        
        deltaAccumulator += gPos;

        RaycastHit hit;
        Ray cameraRay = Camera.main.ScreenPointToRay(mPos + deltaAccumulator);
        if (Physics.Raycast(cameraRay, out hit, Mathf.Infinity, whatIsGround))
        {
            if (summonCursor.position != hit.point)
            {
                summonCursor.position = hit.point;
            }
        }
    }

    void SummonClone(Vector3 spawnPos)
    {
        Instantiate(CrystalIrisPrefab, spawnPos + Vector3.up, Quaternion.identity);
        basicMovementIris.canMove = false;
        
        canSpawn = false;
        inCrystal = true;
    }

    private void OnEnable()
    {
        initiateSummon = summonControls.SummonClone.InitiateSummon;
        initiateSummon.Enable();

        summonLocationMouse = summonControls.SummonClone.SummonLocationMouse;
        summonLocationMouse.Enable();
        summonLocationGamepad = summonControls.SummonClone.SummonLocationGamepad;
        summonLocationGamepad.Enable();

        exitSummonLocation = summonControls.SummonClone.ExitSummonLocation;
        exitSummonLocation.Enable();

        summonClone = summonControls.SummonClone.SummonSpawn;
        summonClone.Enable();
    }

    private void OnDisable()
    {
        initiateSummon.Disable();
        summonLocationMouse.Disable();
        summonLocationGamepad.Disable();
        exitSummonLocation.Disable();
        summonClone.Disable();
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(20, 90, 300, 50), "Clone Prototype: Custom Spawn Location") && !inCrystal)
        {
            prototypeModeText.text = "Prototype Mode: Custom Spawn Location";
            prototypeModeText.color = Color.green;

            customMode = true;
            presetMode = false;
            
            InstantiateSpawnTriggers(false);
            InstantiateCloneSpawn(-1);
        }
        
        if (GUI.Button(new Rect(20, 10, 300, 50), "Clone Prototype: Preset Spawn Location") && !inCrystal)
        {
            prototypeModeText.text = "Prototype Mode: Preset Spawn Location";
            prototypeModeText.color = Color.green;
            
            presetMode = true;
            customMode = false;
            
            InstantiateSpawnTriggers(true);

        }
    }

    private void InstantiateSpawnTriggers(bool instantiate)
    {
        if (instantiate)
        {
            for (int i = 0; i < 3; i++)
            {
                spawnTriggers[i].GetComponent<MeshRenderer>().enabled = true;
            }
        }
        
        else if (!instantiate)
        {
            for (int i = 0; i < 3; i++)
            {
                spawnTriggers[i].GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
    
    private void InstantiateCloneSpawn(int spawnNumber)
    {
        for (int i = 0; i < 3; i++)
        {
            if (spawnNumber == i)
            {
                cloneSpawns[i].GetComponent<MeshRenderer>().enabled = true;
                whatSpawnToSummon[i] = 1;
            }
            else
            {
                cloneSpawns[i].GetComponent<MeshRenderer>().enabled = false;
                whatSpawnToSummon[i] = 0;
            }
        }
    }

    private void CheckSpawnsThenSummon()
    {
        for (int i = 0; i < 3; i++)
        {
            if (whatSpawnToSummon[i] == 1)
            {
                cloneSpawns[i].GetComponent<MeshRenderer>().enabled = false;
                
                SummonClone(cloneSpawns[i].position);
                
                InstantiateSpawnTriggers(false);
                canSpawn = false;
                canInitiate = false;
                inCrystal = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (presetMode && !inCrystal)
        {
            if (other.gameObject.CompareTag("SpawnTrigger1"))
            {
                InstantiateCloneSpawn(0);
            }
            else if (other.gameObject.CompareTag("SpawnTrigger2"))
            {
                InstantiateCloneSpawn(1);
            }
            else if (other.gameObject.CompareTag("SpawnTrigger3"))
            {
                InstantiateCloneSpawn(2);
            }
        }
    }
}
