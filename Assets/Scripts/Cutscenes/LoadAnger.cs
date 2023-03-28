using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LoadAnger : MonoBehaviour
{
    // Input variables
    PlayerControls playerControls;
    public InputAction skipCutscene;
    
    // A to skip Text
    public GameObject aToSkip;
    private bool canSkip;

    // Start is called before the first frame update
    void Awake()
    {
        aToSkip.SetActive(false);
        canSkip = false;
        playerControls = new PlayerControls();
        skipCutscene = playerControls.Cutscene.SkipCutscene;
        skipCutscene.Enable();
        StartCoroutine(OpenMainMenu());
    }

    void Update()
    {
        if (skipCutscene.WasPressedThisFrame() && canSkip)
        {
            StopCoroutine(OpenMainMenu());

            SceneManager.LoadScene("Main Menu");
        }
    }
    
    private IEnumerator OpenMainMenu()
    {
        yield return new WaitForSeconds(3f);

        aToSkip.SetActive(true);
        canSkip = true;
        
        yield return new WaitForSeconds(40f);

        SceneManager.LoadScene("Main Menu");
    }
}
