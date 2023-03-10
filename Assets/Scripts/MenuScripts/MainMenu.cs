using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Sets prototype scene to active and set time scale to normal when play button is pressed.
    public void PlayGame()
    {
        GameManager.Instance.SetLevel(Levels.DEN);
        GameManager.Instance.SetCheckpoint(Levels.DEN, new Vector3(-0.2f, 45f, 37.3f));

        //DataManager.gameData.level = 1;
        //DataManager.gameData.position = new Vector3(-0.2f, 45f, 37.3f);
        
        Time.timeScale = 1f;
        SceneManager.LoadScene("VSCutsceneIntro");
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
