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
    [SerializeField]
    bool transferNow = false;
    bool onlyOnce = false;

    private FadeBlack fadeBlack;

    public Rigidbody physicsBall;

    // Start is called before the first frame update
    void Start()
    {
        fadeBlack = FindObjectOfType<FadeBlack>();
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
        
        StartCoroutine(FadeInOutBlack(1.5f));
    }

    public void ChangeVariableScene(string sceneToTransfer)
    {
        //DataManager.gameData.checkpointed = false;
        //GameManager.Instance.SetLevel(Levels.NA);
        Time.timeScale = 1;
        StartCoroutine(FadeInOutBlackVariable(1.5f, sceneToTransfer));
    }

    void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Player")
        {
            var checkpointData = new CheckpointData();
            checkpointData.room = 1;
            
            if (sceneToTransfer == "AngerFinal")
            {
                GameManager.Instance.SetLevel(Levels.ANG);
                checkpointData.position = new Vector3(-264.42f, 1.87f, 94.82f);
                //DataManager.gameData.position.Set(-264.42f, 1.87f, 94.82f);

                sceneToTransfer = "AngerIntro";
            }

            if (sceneToTransfer == "DepressionFinal")
            {
                GameManager.Instance.SetLevel(Levels.DEP);
                checkpointData.position = new Vector3(467.9f, 154.1f, 114.1f);
                //DataManager.gameData.position.Set(460.67f, 252.07f, 114.31f);

                sceneToTransfer = "DepressionIntro";
            }

            if (sceneToTransfer == "BargainingFinal")
            {
                GameManager.Instance.SetLevel(Levels.BAR);
                checkpointData.position = new Vector3(-300.0f, 0.75f, -136.55f);
                //DataManager.gameData.position.Set(-300.0f, 0.75f, -136.55f);

                sceneToTransfer = "BargainingIntro";
            }

            if (sceneToTransfer == "DenialFinal")
            {
                GameManager.Instance.SetLevel(Levels.DEN);
                checkpointData.position = new Vector3(-0.22f, 45.0f, 37.29f);
                //DataManager.gameData.position.Set(-0.22f, 45.0f, 37.29f);
            }

            if (sceneToTransfer == "CameraPrototyping")
            {
                GameManager.Instance.SetLevel(Levels.NA);
                checkpointData.position = new Vector3(-12.32f, 44.47f, 33.9f);
                //DataManager.gameData.position.Set(-12.32f, 44.47f, 33.9f);
            }

            if (sceneToTransfer == "museum")
            {
                GameManager.Instance.SetLevel(Levels.NA);
                checkpointData.position = new Vector3(11.73f, 0.0f, 10.0f);
                //DataManager.gameData.position.Set(11.73f, 0.0f, 10.0f);
            }

            if (sceneToTransfer == "HubFinal")
            {
                GameManager.Instance.SetLevel(Levels.HUB);
                checkpointData.position = new Vector3(3.218f, 43.83f, 11.162779f);
                //DataManager.gameData.position.Set(2.3f, 42.4f, 16.3f);
                GameManager.Instance.CompleteLevel(level);

                if (SceneManager.GetActiveScene().name == "AngerFinal")
                {
                    sceneToTransfer = "AngerOutro";
                }
                else if (SceneManager.GetActiveScene().name == "BargainingFinal")
                {
                    sceneToTransfer = "BargainingOutro";
                }
                else if (SceneManager.GetActiveScene().name == "DepressionFinal")
                {
                    sceneToTransfer = "DepressionOutro";
                }
                else if (SceneManager.GetActiveScene().name == "DenialFinal")
                {
                    StartCoroutine(endDenial());
                }
            }
            if (sceneToTransfer == "AcceptanceFinalLevel")
            {
                GameManager.Instance.SetLevel(Levels.ACC);
                checkpointData.position = new Vector3(-10.74f, 45.0f, 27.74f);
                //DataManager.gameData.position.Set(-10.74f, 45.0f, 27.74f);

                sceneToTransfer = "AcceptanceIntro";
            }

            GameManager.Instance.SetCheckpoint(checkpointData);
            DataManager.gameData.checkpointed = false;


            if (SceneManager.GetActiveScene().name != "DenialFinal")
            StartCoroutine(FadeInOutBlack(1.5f));
        }

    }

    private IEnumerator endDenial()
    {
        GameManager.Instance.DisableInput();
        physicsBall.AddForce(new Vector3(0, 0, -100000));

        yield return new WaitForSeconds(3);
        
        sceneToTransfer = "DenialOutro";
        StartCoroutine(FadeInOutBlack(1.5f));
    }
    
    public IEnumerator FadeInOutBlack(float waitTime)
    {
        GameManager.Instance.DisableInput();
        fadeBlack.FadeToBlack(waitTime);
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(sceneToTransfer);
    }
    
    public IEnumerator FadeInOutBlackVariable(float waitTime, string sceneToTransfer)
    {
        GameManager.Instance.DisableInput();
        
        fadeBlack.FadeToBlack(waitTime);

        yield return new WaitForSeconds(waitTime);
        
        SceneManager.LoadScene(sceneToTransfer);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transferNow && !onlyOnce)
        {
            Debug.Log("ok");
            onlyOnce = true;
            ChangeScene();
        }
    }
}
