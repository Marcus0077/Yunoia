using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class TriggerEndScene : MonoBehaviour
{
    string sceneToTransfer;
    [SerializeField] Levels level;
    
    private FadeBlack fadeBlack;
    
    void Start()
    {
        fadeBlack = FindObjectOfType<FadeBlack>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (level == Levels.ACC)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(EndGame(2f));
            }
        }
    }

    private IEnumerator EndGame(float waitTime)
    {
        GameManager.Instance.DisableInput();
        fadeBlack.FadeToBlack(waitTime);
        yield return new WaitForSeconds(waitTime);

        var checkpointData = new CheckpointData();
        checkpointData.room = 1;

        //GameManager.Instance.CompleteLevel(level);
        GameManager.Instance.SetCheckpoint(checkpointData);
        DataManager.gameData.checkpointed = false;
        
        sceneToTransfer = "CutsceneEnd";
        SceneManager.LoadScene(sceneToTransfer);
    }
}
