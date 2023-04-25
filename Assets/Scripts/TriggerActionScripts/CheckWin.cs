using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWin : MonoBehaviour
{
    [SerializeField]
    Levels level;
    [SerializeField]
    GameObject portal;
    [SerializeField]
    GameObject postProcess;
    bool complete;
    // Start is called before the first frame update
    void Start()
    {
        complete = GameManager.Instance.GetLevelStatus(level);
        Debug.Log(GameManager.Instance.GetLevelStatus(level));
        if(complete)
        {
            portal.SetActive(false);
            postProcess.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
