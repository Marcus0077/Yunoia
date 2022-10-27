using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Pause menu variables
    public static bool gameIsPaused;
    public GameObject PauseMenuUI;
    
    // Input variables
    PlayerControls playerControls;
    private InputAction pauseGame;
    
    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    void Update()
    {
        // When pause button is pressed:
        // Resume is game is already paused.
        // Pause if game is not paused.
        if (pauseGame.WasPressedThisFrame())
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // Resume game; set normal time and deactivate pause values.
    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    
    // Pause game; freeze time and activate pause values.
    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
    
    // Load main menu when menu button is pressed.
    public void ReturnToMainMenu()
    {
        DataManager.gameData.level = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(0);
    }
    
    // Quit application when quit button is pressed.
    public void QuitGame()
    {
        DataManager.gameData.level = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Quit Button Works!");
        DataManager.WriteFile();
        Application.Quit();
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteAll();
    }

    // Enable input action map controls.
    private void OnEnable()
    {
        pauseGame = playerControls.PauseMenu.PauseGame;
        pauseGame.Enable();

    }

    // Disable input action map controls.
    private void OnDisable()
    {
        pauseGame.Disable();

    }
}
