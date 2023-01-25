using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CheckpointRestart : MonoBehaviour
{
    PlayerControls checkpointControls;
    
    private InputAction restartAction;
    private LimitedMovementCam limitedMovementCam;

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    void Awake()
    {
        limitedMovementCam = GameObject.FindObjectOfType<LimitedMovementCam>();
        
        if (DataManager.gameData.checkpointed)
        {
            Debug.Log(DataManager.gameData.level);
            // limitedMovementCam.GetCurrentCameraData(DataManager.gameData.level);
            // limitedMovementCam.SetCurrentPlayer(GameObject.FindGameObjectWithTag("Player"));
            
            GameObject.FindWithTag("StateDrivenCam").GetComponent<Animator>().Play("Room" + DataManager.gameData.level);
            transform.position = DataManager.gameData.position;
        }
        checkpointControls = new PlayerControls();
        restartAction = checkpointControls.Checkpoint.Restart;
        restartAction.performed += ctx => Restart();
    }

    private void OnEnable()
    {
        //restartAction.Enable();
    }

    private void OnDisable()
    {
        restartAction.Disable();
    }
}
