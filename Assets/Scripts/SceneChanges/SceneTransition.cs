using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    string sceneToTransfer;
    [SerializeField]
    Levels level;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void RestartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ChangeScene()
    {
        if(SceneManager.GetActiveScene().name != sceneToTransfer)
        {
            DataManager.gameData.checkpointed = false;
            GameManager.Instance.SetLevel(level);
        }
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneToTransfer);
    }

    public void ChangeVariableScene(string sceneToTransfer)
    {
        DataManager.gameData.checkpointed = false;
        GameManager.Instance.SetLevel(Levels.NA);
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneToTransfer);
    }

    void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Player")
        {
            if (sceneToTransfer == "AngerFinal")
            {
                GameManager.Instance.SetLevel(Levels.ANG);
                GameManager.Instance.SetCheckpoint(Levels.ANG, new Vector3(-264.42f, 1.87f, 94.82f));
                DataManager.gameData.checkpointed = false;
                //DataManager.gameData.position.Set(-264.42f, 1.87f, 94.82f);
            }

            if (sceneToTransfer == "DepressionFinal")
            {
                GameManager.Instance.SetLevel(Levels.DEP);
                GameManager.Instance.SetCheckpoint(Levels.DEP, new Vector3(460.67f, 252.07f, 114.31f));
                DataManager.gameData.checkpointed = false;
                //DataManager.gameData.position.Set(460.67f, 252.07f, 114.31f);
            }

            if (sceneToTransfer == "BargainingFinal")
            {
                GameManager.Instance.SetLevel(Levels.BAR);
                GameManager.Instance.SetCheckpoint(Levels.BAR, new Vector3(-300.0f, 0.75f, -136.55f));
                DataManager.gameData.checkpointed = false;
                //DataManager.gameData.position.Set(-300.0f, 0.75f, -136.55f);
            }

            if (sceneToTransfer == "DenialFinal")
            {
                GameManager.Instance.SetLevel(Levels.DEN);
                GameManager.Instance.SetCheckpoint(Levels.DEN, new Vector3(-0.22f, 45.0f, 37.29f));
                DataManager.gameData.checkpointed = false;
                //DataManager.gameData.position.Set(-0.22f, 45.0f, 37.29f);
            }

            if (sceneToTransfer == "CameraPrototyping")
            {
                GameManager.Instance.SetLevel(Levels.NA);
                GameManager.Instance.SetCheckpoint(Levels.NA, new Vector3(-12.32f, 44.47f, 33.9f));
                DataManager.gameData.checkpointed = false;
                //DataManager.gameData.position.Set(-12.32f, 44.47f, 33.9f);
            }

            if (sceneToTransfer == "museum")
            {
                GameManager.Instance.SetLevel(Levels.NA);
                GameManager.Instance.SetCheckpoint(Levels.NA, new Vector3(11.73f, 0.0f, 10.0f));
                DataManager.gameData.checkpointed = false;
                //DataManager.gameData.position.Set(11.73f, 0.0f, 10.0f);
            }

            if(sceneToTransfer == "HubFinal")
            {
                GameManager.Instance.SetLevel(Levels.HUB);
                GameManager.Instance.SetCheckpoint(Levels.HUB, new Vector3(2.3f, 42.4f, 16.3f));
                DataManager.gameData.checkpointed = false;
                //DataManager.gameData.position.Set(2.3f, 42.4f, 16.3f);
                GameManager.Instance.CompleteLevel(level);
            }

            SceneManager.LoadScene(sceneToTransfer);
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
