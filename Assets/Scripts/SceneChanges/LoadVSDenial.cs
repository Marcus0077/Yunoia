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
        StartCoroutine(LoadDenial());
    }

    void Update()
    {
        if (skipCutscene.WasPressedThisFrame() && canSkip)
        {
            StopCoroutine(LoadDenial());

            SceneManager.LoadScene("VSDenial");
        }
    }
    
    private IEnumerator LoadDenial()
    {
        yield return new WaitForSeconds(3f);

        aToSkip.SetActive(true);
        canSkip = true;
        
        yield return new WaitForSeconds(58f);

        SceneManager.LoadScene("VSDenial");
    }
}
