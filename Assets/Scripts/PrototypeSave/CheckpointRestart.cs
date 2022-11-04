using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CheckpointRestart : MonoBehaviour
{
    PlayerControls checkpointControls;
    private InputAction restartAction;

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }

    void Awake()
    {
        if (DataManager.gameData.checkpointed)
        {
            transform.position = DataManager.gameData.position;
        }
        checkpointControls = new PlayerControls();
        restartAction = checkpointControls.Checkpoint.Restart;
        restartAction.performed += ctx => Restart();
    }

    private void OnEnable()
    {
        restartAction.Enable();
    }

    private void OnDisable()
    {
        restartAction.Disable();
    }
}
