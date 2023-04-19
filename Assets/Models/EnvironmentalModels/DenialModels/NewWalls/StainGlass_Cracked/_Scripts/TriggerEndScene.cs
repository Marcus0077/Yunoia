using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class TriggerEndScene : MonoBehaviour
{
    string sceneToTransfer;
    [SerializeField] Levels level;

    void OnTriggerEnter(Collider other)
    {
        if (level == Levels.ACC)
        {
            if (other.tag == "Player")
            {
                var checkpointData = new CheckpointData();
                checkpointData.room = 1;

                GameManager.Instance.CompleteLevel(level);

                GameManager.Instance.SetCheckpoint(checkpointData);
                DataManager.gameData.checkpointed = false;

                sceneToTransfer = "CutsceneEnd";
                SceneManager.LoadScene(sceneToTransfer);
            }
        }
        else if (level == Levels.DEN)
        {
            if (other.tag == "Player")
            {
                GetComponent<Animator>().SetTrigger("End");
            }
        }
    }
}
