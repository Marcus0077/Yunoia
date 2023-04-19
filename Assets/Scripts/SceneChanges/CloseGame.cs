using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CloseGame : MonoBehaviour
{
    PlayerControls sceneChanges;
    public InputAction closeGame;

    void FixedUpdate()
    {
        if (closeGame.IsPressed())
        {
            Application.Quit();
        }
    }

    private void OnEnable()
    {
        sceneChanges = new PlayerControls();

        closeGame = sceneChanges.Scenes.CloseGame;
        closeGame.Enable();
    }

    private void OnDisable()
    {
        closeGame.Disable();
    }
}
