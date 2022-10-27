using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheckpointRestart : MonoBehaviour
{
    PlayerControls checkpointControls;
    private InputAction restartAction;

    void Restart()
    {
        if(PlayerPrefs.GetInt("CheckpointSaved") != 0)
        {
            transform.position = new Vector3(PlayerPrefs.GetFloat("TempX"), PlayerPrefs.GetFloat("TempY"), PlayerPrefs.GetFloat("TempZ"));
        }
        
    }

    void Awake()
    {
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
