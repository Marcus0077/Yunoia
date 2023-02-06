using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            SceneManager.LoadScene(sceneToTransfer);
            if(sceneToTransfer == "HubFinal")
            {
                GameManager.Instance.CompleteLevel(level);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
