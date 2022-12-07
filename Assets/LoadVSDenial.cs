using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LoadVSDenial : MonoBehaviour
{
    // Input variables
    PlayerControls playerControls;
    public InputAction skipCutscene;
    
    // Start is called before the first frame update
    void Awake()
    {
        playerControls = new PlayerControls();
        skipCutscene = playerControls.Cutscene.SkipCutscene;
        skipCutscene.Enable();
        StartCoroutine(LoadDenial());
    }

    void Update()
    {
        if (skipCutscene.WasPressedThisFrame())
        {
            StopCoroutine(LoadDenial());
            SceneManager.LoadScene("VSDenial");
        }
    }
    
    private IEnumerator LoadDenial()
    {
        yield return new WaitForSeconds(61f);

        SceneManager.LoadScene("VSDenial");
    }
}
