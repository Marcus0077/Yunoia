using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PauseMenu : MonoBehaviour
{
    // Input variables
    public PlayerControls playerControls;
    public InputAction pause;
    public InputAction menuMove;

    public GameObject pauseMenu, BGBlur, abilityIcons;
    public bool isPaused;

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

    private void PauseVideo()
    {
        if (GameObject.FindWithTag("Outro") != null)
        {
            if (GameObject.FindWithTag("Outro").GetComponent<VideoPlayer>().isPlaying)
                GameObject.FindWithTag("Outro").GetComponent<VideoPlayer>().Pause();
        }
    }

    private void ResumeVideo()
    {
        if (GameObject.FindWithTag("Outro") != null)
        {
            if (GameObject.FindWithTag("Outro").GetComponent<VideoPlayer>().isPaused)
                GameObject.FindWithTag("Outro").GetComponent<VideoPlayer>().Play();
        }
    }

    // Disable movement and ability input.
    public void DisableInput()
    {
        GameObject player;
        GameObject clone;
        
        player = GameObject.FindGameObjectWithTag("Player");
        clone = GameObject.FindGameObjectWithTag("Clone");

        if (player != null)
        {
            // Disable input for the player.
            if (player.GetComponent<BasicMovement>() != null)
            {
                player.GetComponent<BasicMovement>().playerControls.Disable();
            }

            if (player.GetComponent<SummonClone>() != null)
            {
                player.GetComponent<SummonClone>().summonAClone.Disable();
            }

            if (player.GetComponent<Grapple>() != null)
            {
                player.GetComponent<Grapple>().shootHook.Disable();
                player.GetComponent<Grapple>().extendGrapple.Disable();
                player.GetComponent<Grapple>().cancelHook.Disable();
            }

            if (player.GetComponent<AbilityPush>() != null)
            {
                player.GetComponent<AbilityPush>().pushControls.Disable();
            }
        }

        if (clone != null)
        {
            //Disable input for the clone.
            if (clone.GetComponent<BasicMovement>() != null)
            {
                clone.GetComponent<BasicMovement>().playerControls.Disable();
            }

            if (clone.GetComponent<SummonClone>() != null)
            {
                clone.GetComponent<SummonClone>().summonAClone.Disable();
            }

            if (clone.GetComponent<Grapple>() != null)
            {
                clone.GetComponent<Grapple>().shootHook.Disable();
                clone.GetComponent<Grapple>().extendGrapple.Disable();
                clone.GetComponent<Grapple>().cancelHook.Disable();
            }

            if (clone.GetComponent<AbilityPush>() != null)
            {
                clone.GetComponent<AbilityPush>().pushControls.Disable();
            }
        }
    }
    
    // Enable movement and ability input.
    public void EnableInput()
    {
        Debug.Log("enabled runs.");
        
        GameObject player;
        GameObject clone;
        
        player = GameObject.FindGameObjectWithTag("Player");
        clone = GameObject.FindGameObjectWithTag("Clone");

        if (player != null)
        {
            // Enable input for the player.
            if (player.GetComponent<BasicMovement>() != null)
            {
                player.GetComponent<BasicMovement>().playerControls.Enable();
            }

            if (player.GetComponent<SummonClone>() != null)
            {
                player.GetComponent<SummonClone>().summonAClone.Enable();
            }

            if (player.GetComponent<Grapple>() != null)
            {
                player.GetComponent<Grapple>().shootHook.Enable();
                player.GetComponent<Grapple>().extendGrapple.Enable();
                player.GetComponent<Grapple>().cancelHook.Enable();
            }

            if (player.GetComponent<AbilityPush>() != null)
            {
                player.GetComponent<AbilityPush>().pushControls.Enable();
            }
        }

        if (clone != null)
        {
            //Enable input for the clone.
            if (clone.GetComponent<BasicMovement>() != null)
            {
                clone.GetComponent<BasicMovement>().playerControls.Enable();
            }

            if (clone.GetComponent<SummonClone>() != null)
            {
                clone.GetComponent<SummonClone>().summonAClone.Enable();
            }

            if (clone.GetComponent<Grapple>() != null)
            {
                clone.GetComponent<Grapple>().shootHook.Enable();
                clone.GetComponent<Grapple>().extendGrapple.Enable();
                clone.GetComponent<Grapple>().cancelHook.Enable();
            }

            if (clone.GetComponent<AbilityPush>() != null)
            {
                clone.GetComponent<AbilityPush>().pushControls.Enable();
            }
        }
    }

    public void Pause()
    {
        PauseVideo();
        //DisableInput();
        GameManager.Instance.DisableInput();
        DataManager.WriteFile();

        pauseMenu.SetActive(true);
        BGBlur.SetActive(true);
        
        if (abilityIcons != null)
            abilityIcons.SetActive(false);
        
        Time.timeScale = 0;

        isPaused = true;
    }

    public void Resume()
    {
        ResumeVideo();

        if (GameObject.FindGameObjectWithTag("Outro") != null)
        {
            if (!GameObject.FindGameObjectWithTag("Outro").activeInHierarchy)
                //EnableInput();
                GameManager.Instance.EnableInput();
        }
        else
        {
            //EnableInput();
            GameManager.Instance.EnableInput();
        }

        pauseMenu.SetActive(false);
        BGBlur.SetActive(false);
        
        if (abilityIcons != null)
            abilityIcons.SetActive(true);
        
        Time.timeScale = 1;

        pauseMenu.GetComponent<MenuTraverse>().ExitCurrent();
        isPaused = false;
    }
    
    private void OnDisable()
    {
        pause.Disable();
    }

    public void QuitGame()
    {
        Debug.Log("Quit Button Works!");
        Application.Quit();
    }
}
