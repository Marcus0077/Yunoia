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

    public GameObject pauseMenu, BGBlur;
    private bool isPaused;

    public MenuTransition menuTransition;

    private void Start()
    {
        playerControls = new PlayerControls();
        pause = playerControls.PauseMenu.PauseGame;
        pause.Enable();
        
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    private void Update()
    {
        PauseGame();
    }

    public void PauseGame()
    {
        if (pause.WasPressedThisFrame())
        {
            if (!isPaused && !menuTransition.inSettings)
            {
                Pause();
            }
            else if (isPaused && !menuTransition.inSettings)
            {
                Resume();
            }
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        BGBlur.SetActive(true);
        Time.timeScale = 0;

        isPaused = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        BGBlur.SetActive(false);
        Time.timeScale = 1;

        isPaused = false;
    }
    
    private void OnDisable()
    {
        pause.Disable();
    }
}
