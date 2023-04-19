using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SpawnAcceptance : MonoBehaviour
{
    public GameObject acceptTeleport;

    // Start is called before the first frame update

    private void FixedUpdate()
    {

        if (DataManager.gameData.levelCompletion[(int)Levels.ANG]
            && DataManager.gameData.levelCompletion[(int)Levels.BAR]
            && DataManager.gameData.levelCompletion[(int)Levels.DEP])
        {
            acceptTeleport.SetActive(true);
        }
        else
        {
            acceptTeleport.SetActive(false);
        }
    }
}
