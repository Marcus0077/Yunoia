using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    string sceneToTransfer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            SceneManager.LoadScene(sceneToTransfer);
        }

        {
        if (other.tag == "Depression")
        {
           SceneManager.LoadScene("DepressionFinal");
        }
         }
          {
        if (other.tag == "Bargaining")
        {
           SceneManager.LoadScene("BargainingFinal");
        }
         }
          {
        if (other.tag == "Anger")
        {
           SceneManager.LoadScene("AngerFinal");
        }
         }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
