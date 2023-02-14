using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneChanger : MonoBehaviour
{

    PlayerControls sceneChanges;

    public InputAction angerScene;
    public InputAction bargainingScene;
    public InputAction depressionScene;
    public InputAction denialScene;
    public InputAction acceptanceScene;
    public InputAction hubScene;
    public InputAction museumScene;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (angerScene.IsPressed())
        {
            SceneManager.LoadScene("AngerFinal");
        }

        if (bargainingScene.IsPressed())
        {
            SceneManager.LoadScene("BargainingFinal");
        }

        if (depressionScene.IsPressed())
        {
            SceneManager.LoadScene("DepressionFinal");
        }

        if (denialScene.IsPressed())
        {
            SceneManager.LoadScene("DenialFinal");
        }

        if (acceptanceScene.IsPressed())
        {
            SceneManager.LoadScene("AcceptanceFinal");
        }

        if (hubScene.IsPressed())
        {
            SceneManager.LoadScene("HubFinal");
        }

        if (museumScene.IsPressed())
        {
            SceneManager.LoadScene("museum");
        }
    }

    private void OnEnable()
    {
        sceneChanges = new PlayerControls();

        angerScene = sceneChanges.Scenes.AngerScene;
        angerScene.Enable();

        bargainingScene = sceneChanges.Scenes.BargainingScene;
        bargainingScene.Enable();

        depressionScene = sceneChanges.Scenes.DepressionScene;
        depressionScene.Enable();

        denialScene = sceneChanges.Scenes.DenialScene;
        denialScene.Enable();

        acceptanceScene = sceneChanges.Scenes.AcceptanceScene;
        acceptanceScene.Enable();

        hubScene = sceneChanges.Scenes.HubScene;
        hubScene.Enable();

        museumScene = sceneChanges.Scenes.MuseumScene;
        museumScene.Enable();
    }

    private void OnDisable()
    {
        angerScene.Disable();
        bargainingScene.Disable();
        depressionScene.Disable();
        denialScene.Disable();
        acceptanceScene.Disable();
        hubScene.Disable();
        museumScene.Disable();
    }
}
