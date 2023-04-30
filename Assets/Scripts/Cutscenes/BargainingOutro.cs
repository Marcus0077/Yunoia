using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class BargainingOutro : MonoBehaviour
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
        StartCoroutine(OpenScene());
    }

    void Update()
    {
        if (skipCutscene.WasPressedThisFrame() && canSkip)
        {
            StopCoroutine(OpenScene());

            SceneManager.LoadScene("HubFinal");
        }
    }
    
    private IEnumerator OpenScene()
    {
        yield return new WaitForSeconds(3f);

        aToSkip.SetActive(true);
        canSkip = true;
        
        yield return new WaitForSeconds(14f);

        SceneManager.LoadScene("HubFinal");
    }
}
