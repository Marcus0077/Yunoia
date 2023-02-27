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
    public InputAction closeGame;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (angerScene.IsPressed())
        {
            DataManager.gameData.checkpointed = false;
            DataManager.gameData.level = 1;
            SceneManager.LoadScene("AngerFinal");
        }

        if (bargainingScene.IsPressed())
        {
            DataManager.gameData.checkpointed = false;
            DataManager.gameData.level = 1;
            SceneManager.LoadScene("BargainingFinal");
        }

        if (depressionScene.IsPressed())
        {
            DataManager.gameData.checkpointed = false;
            DataManager.gameData.level = 1;
            SceneManager.LoadScene("DepressionFinal");
        }

        if (denialScene.IsPressed())
        {
            DataManager.gameData.checkpointed = false;
            DataManager.gameData.level = 1;
            SceneManager.LoadScene("DenialFinal");
        }

        if (acceptanceScene.IsPressed())
        {
            DataManager.gameData.checkpointed = false;
            DataManager.gameData.level = 1;
            SceneManager.LoadScene("AcceptanceFinal");
        }

        if (hubScene.IsPressed())
        {
            DataManager.gameData.checkpointed = false;
            DataManager.gameData.level = 1;
            SceneManager.LoadScene("HubFinal");
        }

        if (museumScene.IsPressed())
        {
            DataManager.gameData.checkpointed = false;
            DataManager.gameData.level = 1;
            SceneManager.LoadScene("museum");
        }

        if (closeGame.IsPressed())
        {
            Application.Quit();
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

        closeGame = sceneChanges.Scenes.CloseGame;
        closeGame.Enable();
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
        closeGame.Disable();
    }
}
