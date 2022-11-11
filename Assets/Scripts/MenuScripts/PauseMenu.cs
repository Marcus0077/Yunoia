using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Input variables
    PlayerControls playerControls;
    public InputAction pause;

    public GameObject pauseMenu;
    private bool isPaused;

    private void Start()
    {
        playerControls = new PlayerControls();
        pause = playerControls.PauseMenu.PauseGame;
        
        isPaused = false;
    }

    private void Update()
    {
        if (pause.WasPressedThisFrame())
        {
            
            Debug.Log("works");
            
            if (!isPaused)
            {
                
            }
            else if (isPaused)
            {
                
            }
        }
    }

    private void OnEnable()
    {
        
        pause.Enable();
    }

    private void OnDisable()
    {
        pause.Disable();
    }
}
