using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DeleteSettings()
    {
        GameManager.Instance.NewGame();
        GameManager.Instance.menuTraverse.ResetSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
