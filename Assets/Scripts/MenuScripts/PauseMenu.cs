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
                DisableInput();
                Pause();
            }
            else if (isPaused && !menuTransition.inSettings)
            {
                EnableInput();
                Resume();
            }
        }
    }

    // Disable movement and ability input.
    private void DisableInput()
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
                player.GetComponent<BasicMovement>().move.Disable();
                player.GetComponent<BasicMovement>().jump.Disable();
                player.GetComponent<BasicMovement>().dash.Disable();
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
                player.GetComponent<AbilityPush>().pushAction.Disable();
            }
        }

        if (clone != null)
        {
            //Disable input for the clone.
            if (clone.GetComponent<BasicMovement>() != null)
            {
                clone.GetComponent<BasicMovement>().move.Disable();
                clone.GetComponent<BasicMovement>().jump.Disable();
                clone.GetComponent<BasicMovement>().dash.Disable();
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
                clone.GetComponent<AbilityPush>().pushAction.Disable();
            }
        }
    }
    
    // Enable movement and ability input.
    private void EnableInput()
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
                player.GetComponent<BasicMovement>().move.Enable();
                player.GetComponent<BasicMovement>().jump.Enable();
                player.GetComponent<BasicMovement>().dash.Enable();
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
                player.GetComponent<AbilityPush>().pushAction.Enable();
            }
        }

        if (clone != null)
        {
            //Enable input for the clone.
            if (clone.GetComponent<BasicMovement>() != null)
            {
                clone.GetComponent<BasicMovement>().move.Enable();
                clone.GetComponent<BasicMovement>().jump.Enable();
                clone.GetComponent<BasicMovement>().dash.Enable();
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
                clone.GetComponent<AbilityPush>().pushAction.Enable();
            }
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        BGBlur.SetActive(true);
        abilityIcons.SetActive(false);
        Time.timeScale = 0;

        isPaused = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        BGBlur.SetActive(false);
        abilityIcons.SetActive(true);
        Time.timeScale = 1;
        EnableInput();

        pauseMenu.GetComponent<MenuTraverse>().ExitCurrent();
        isPaused = false;
    }
    
    private void OnDisable()
    {
        pause.Disable();
    }
}
