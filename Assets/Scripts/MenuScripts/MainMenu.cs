using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;

        if (DataManager.gameData != null)
        {
            if (DataManager.gameData.levelCompletion[(int)Levels.ACC])
            {
                GameManager.Instance.NewGame();
            }
        }
    }

    // Sets prototype scene to active and set time scale to normal when play button is pressed.
    public void PlayGame()
    {
        if(GameManager.Instance.GetFloat(Settings.FIRSTPLAY) >= 0)
        {
            DataManager.WriteFile();
            GameManager.Instance.SetLevel(Levels.DEN);
            GameManager.Instance.SetCheckpoint(Levels.DEN, new Vector3(-20.25f, 46.8f, 57.5f));

            //DataManager.gameData.level = 1;
            //DataManager.gameData.position = new Vector3(-0.2f, 45f, 37.3f);

            Time.timeScale = 1f;
            GameManager.Instance.SetFloat(Settings.FIRSTPLAY, -1);
            SceneManager.LoadScene("VSCutsceneIntro");
            
        }
        else
        {
            GameManager.Instance.SetLevel(DataManager.gameData.level);
            DataManager.gameData.checkpointed = true;
            SceneManager.LoadScene(GameManager.Instance.levelNames[GameManager.Instance.currentLevel]);
        }
        
    }
    
    // Exits application when quit button is pressed.
    public void QuitGame()
    {
        Debug.Log("Quit Button Works!");
        Application.Quit();
    }

    public void ToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}
