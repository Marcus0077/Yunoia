using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    
    void Awake()
    {
        summonControls = new PlayerControls();
        canInitiate = true;
        canSummon = false;
        summonCursor.gameObject.SetActive(false);
        basicMovementIris = FindObjectOfType<BasicMovementIris>();
        deltaAccumulator = basicMovementIris.irisRB.transform.position;
    }
    
    void FixedUpdate()
    {
        InitiateSummon();
        
        if (canSummon == true)
        {
            SummonLocation();
        }
    }

    void InitiateSummon()
    {
        if (initiateSummon.IsPressed() && canInitiate)
        {
            canInitiate = false;
            canSummon = true;
            basicMovementIris.canMove = false;
        }
    }

    void SummonLocation()
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
}
