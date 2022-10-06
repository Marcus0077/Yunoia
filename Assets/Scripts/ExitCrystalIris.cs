using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExitCrystalIris : MonoBehaviour
{
    PlayerControls summonControls;
    private InputAction exitCrystalIris;
    
    private BasicMovementIris basicMovementIris;
    private SummonCrystalIris summonCrystalIris;
    
    void Awake()
    {
        summonControls = new PlayerControls();
        basicMovementIris = FindObjectOfType<BasicMovementIris>();
        summonCrystalIris = FindObjectOfType<SummonCrystalIris>();
    }
    
    void FixedUpdate()
    {
        if (exitCrystalIris.IsPressed())
        {
            basicMovementIris.canMove = true;
            summonCrystalIris.canInitiate = true;
            Destroy(this.gameObject);
        }
    }
    
    private void OnEnable()
    {
        exitCrystalIris = summonControls.SummonClone.ExitCrystalIris;
        exitCrystalIris.Enable();
    }

    private void OnDisable()
    {
        exitCrystalIris.Disable();
    }
}
