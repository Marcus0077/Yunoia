using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Sets prototype scene to active and set time scale to normal when play button is pressed.
    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    // Exits application when quit button is pressed.
    public void QuitGame()
    {
        Debug.Log("Quit Button Works!");
        Application.Quit();
    }
}
