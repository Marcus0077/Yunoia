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
            GameManager.Instance.SetLevel(Levels.ANG);
            var checkpointData = new CheckpointData();
            checkpointData.room = 1;
            checkpointData.position = new Vector3(-264.42f, 1.87f, 94.82f);
            GameManager.Instance.SetCheckpoint(checkpointData);
            SceneManager.LoadScene("AngerFinal");
        }

        if (bargainingScene.IsPressed())
        {
            DataManager.gameData.checkpointed = false;
            GameManager.Instance.SetLevel(Levels.BAR);
            var checkpointData = new CheckpointData();
            checkpointData.room = 1;
            checkpointData.position = new Vector3(-300.0f, 0.75f, -136.55f);
            GameManager.Instance.SetCheckpoint(checkpointData);
            SceneManager.LoadScene("BargainingFinal");
        }

        if (depressionScene.IsPressed())
        {
            DataManager.gameData.checkpointed = false;
            GameManager.Instance.SetLevel(Levels.DEP);
            var checkpointData = new CheckpointData();
            checkpointData.room = 1;
            checkpointData.position = new Vector3(467.9f, 154.1f, 114.1f);
            GameManager.Instance.SetCheckpoint(checkpointData);
            SceneManager.LoadScene("DepressionFinal");
        }

        if (denialScene.IsPressed())
        {
            DataManager.gameData.checkpointed = false;
            GameManager.Instance.SetLevel(Levels.DEN);
            var checkpointData = new CheckpointData();
            checkpointData.room = 1;
            checkpointData.position = new Vector3(-0.22f, 45.0f, 37.29f);
            GameManager.Instance.SetCheckpoint(checkpointData);
            SceneManager.LoadScene("DenialFinal");
        }

        if (acceptanceScene.IsPressed())
        {
            DataManager.gameData.checkpointed = false;
            GameManager.Instance.SetLevel(Levels.ACC);
            var checkpointData = new CheckpointData();
            checkpointData.room = 1;
            checkpointData.position = new Vector3(-10.74f, 45.0f, 27.74f);
            GameManager.Instance.SetCheckpoint(checkpointData);
            SceneManager.LoadScene("AcceptanceFinalLevel");
        }

        if (hubScene.IsPressed())
        {
            DataManager.gameData.checkpointed = false;
            GameManager.Instance.SetLevel(Levels.HUB);
            var checkpointData = new CheckpointData();
            checkpointData.room = 1;
            checkpointData.position = new Vector3(3.218f, 43.83f, 11.162779f);
            GameManager.Instance.SetCheckpoint(checkpointData);
            SceneManager.LoadScene("HubFinal");
        }

        if (museumScene.IsPressed())
        {
            DataManager.gameData.checkpointed = false;
            GameManager.Instance.SetLevel(Levels.NA);
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
