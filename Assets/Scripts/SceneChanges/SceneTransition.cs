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

    public void ChangeScene(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Player")
        {
            if (sceneToTransfer == "AngerFinal")
            {
                DataManager.gameData.position.Set(-264.42f, 1.87f, 94.82f);
            }

            if (sceneToTransfer == "DepressionFinal")
            {
                DataManager.gameData.position.Set(460.67f, 252.07f, 114.31f);
            }

            if (sceneToTransfer == "BargainingFinal")
            {
                DataManager.gameData.position.Set(0.0f, 1.685f, -1.85f);
            }

            if (sceneToTransfer == "VSDenial")
            {
                DataManager.gameData.position.Set(-0.22f, 45.0f, 37.29f);
            }

            if (sceneToTransfer == "CameraPrototyping")
            {
                DataManager.gameData.position.Set(-12.32f, 44.47f, 33.9f);
            }

            if (sceneToTransfer == "museum")
            {
                DataManager.gameData.position.Set(11.73f, 0.0f, 10.0f);
            }

            if(sceneToTransfer == "HubFinal" && SceneManager.GetActiveScene().name != "VSDenial")
            {
                DataManager.gameData.position.Set(0.92f, 47.07f, 19.93f);
                GameManager.Instance.CompleteLevel(level);
            }
            
            if(sceneToTransfer == "HubFinal" && SceneManager.GetActiveScene().name == "VSDenial")
            {
                DataManager.gameData.position.Set(0.92f, 47.07f, 19.93f);
            }

            SceneManager.LoadScene(sceneToTransfer);
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
