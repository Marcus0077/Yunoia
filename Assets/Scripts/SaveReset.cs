using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveReset : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            if (File.Exists(DataManager.saveFilePath))
            {
                File.Delete(DataManager.saveFilePath);
            }
        }
    }
}
