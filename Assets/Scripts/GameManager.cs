using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    
    
    // Strings for settings and stage names
    private static readonly string Firstplay = "First Play";
    private static readonly string BGMPref = "BGM Pref";
    private static readonly string SFXPref = "SFX Pref";
    private static readonly string MasPref = "Mas Pref";
    private static readonly string Sensitivity = "Sensitivity";
    private static readonly string Rumble = "Rumble";
    private static readonly string Depr = "Depression";
    private static readonly string Barg = "Bargaining";
    private static readonly string Anger = "Anger";
    private static readonly string Denial = "Denial";
    private static readonly string Hub = "Hub";
    private static readonly string TextColor = "TextColor";

    public string[] statics;
    public string[] levelNames;
    public float[] settings;

    public static GameManager instance;
    public bool ghost = false, menuCursor = false, textColor = false, rebinded = false;
    [SerializeField]
    GameObject player, ghostObj;
    GameObject spawnedGhost;
    List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
    public List<IAbility> abilities = new List<IAbility>();
    public float time;
    public string controlScheme, rebinds;
    public int currentLevel;
    public MenuTraverse menuTraverse;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    //Scene needs to be reloaded to turn off text color
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!textColor)
            return;
        texts = new List<TextMeshProUGUI>(FindObjectsOfType<TextMeshProUGUI>());
        texts.ForEach(text => text.color = ConvertFloatToHex(settings[(int)Settings.TXTCLR]));
    }

    public void SetLevel(int value)
    {
        currentLevel = value;
    }

    public void SetLevel(Levels value)
    {
        currentLevel = (int)value;
    }

    public void SetCheckpoint(int value, Vector3 pos)
    {
        DataManager.gameData.checkpointDatas[value].position = pos;
    }

    public void SetCheckpoint(Levels value, Vector3 pos)
    {
        DataManager.gameData.checkpointDatas[(int)value].position = pos;
    }

    public void SetCheckpoint(CheckpointData data)
    {
        DataManager.gameData.checkpointDatas[currentLevel] = data;
    }

    public CheckpointData GetCheckpoint()
    {
        return DataManager.gameData.checkpointDatas[currentLevel];
    }

    public void GetInputs(PlayerControls actions)
    {
        if(!rebinded)
        {
            if (rebinds == "")
                rebinds = PlayerPrefs.GetString("Rebinds");
            if (rebinds != "")
                actions.LoadBindingOverridesFromJson(rebinds);//player.GetComponent<PlayerInput>().actions.LoadBindingOverridesFromJson(rebinds);
            rebinded = true;
        }
    }

    public void GetAllInputs()
    {
        foreach(IAbility ability in abilities)
        {
            ability.ResetRebind();
        }
    }

    public void SetColor()
    {
        if (!textColor)
            return;
        texts = new List<TextMeshProUGUI>(FindObjectsOfType<TextMeshProUGUI>());
        Color newColor = ConvertFloatToHex(settings[(int)Settings.TXTCLR]);
        texts.ForEach(text => text.color = newColor);
    }

    //Color conversion methods (uses bytes for easier storage)
    public float ConvertHexToFloat(Color color)
    {
        Color32 color_byte = new Color32((byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), (byte)(color.a * 255));
        return float.Parse(color_byte.r.ToString("000") + color_byte.g.ToString("000") + color_byte.b.ToString("000"));
    }

    public Color ConvertFloatToHex(float value)
    {
        string colorString = value.ToString("000000000");
        return new Color32(Byte.Parse(colorString.Substring(0, 3)), Byte.Parse(colorString.Substring(3, 3)), Byte.Parse(colorString.Substring(6, 3)), 255);
    }

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
            DataManager.ReadFile();
            DataManager.gameData.checkpointed = false;
            statics = new string[] { Firstplay, BGMPref, SFXPref, MasPref, Sensitivity, TextColor, Rumble };
            levelNames = new string[] { Depr, Barg, Anger, Denial };
            settings = new float[System.Enum.GetValues(typeof(Settings)).Length];
            if (player == null)
                player = GameObject.FindGameObjectWithTag("Player");
            SceneManager.sceneLoaded += OnSceneLoaded;
            // Set variables ready for a new game (create a button that sets Firstplay to 0 for new game?)
            if (PlayerPrefs.GetFloat(Firstplay) == 0)
            {
                textColor = false;
                PlayerPrefs.SetFloat(Sensitivity, .5f);
                PlayerPrefs.SetFloat(MasPref, .5f);
                PlayerPrefs.SetFloat(BGMPref, 1);
                PlayerPrefs.SetFloat(SFXPref, 1);
                PlayerPrefs.SetFloat(TextColor, -1);
                PlayerPrefs.SetFloat(Rumble, 1);
                PlayerPrefs.SetString("Rebinds", "");
                settings[(int)Settings.SENSE] = PlayerPrefs.GetFloat(Sensitivity);
                settings[(int)Settings.MAS] = PlayerPrefs.GetFloat(MasPref);
                settings[(int)Settings.BGM] = PlayerPrefs.GetFloat(BGMPref);
                settings[(int)Settings.SFX] = PlayerPrefs.GetFloat(SFXPref);
                settings[(int)Settings.TXTCLR] = PlayerPrefs.GetFloat(TextColor);
                settings[(int)Settings.RUMB] = PlayerPrefs.GetFloat(Rumble);
                PlayerPrefs.SetFloat(Firstplay, -1);
                //PlayerPrefs.SetInt(Depr, 0);
                //PlayerPrefs.SetInt(Barg, 0);
                //PlayerPrefs.SetInt(Anger, 0);
            }
            else
            {
                settings[(int)Settings.SENSE] = PlayerPrefs.GetFloat(Sensitivity);
                settings[(int)Settings.MAS] = PlayerPrefs.GetFloat(MasPref);
                settings[(int)Settings.BGM] = PlayerPrefs.GetFloat(BGMPref);
                settings[(int)Settings.SFX] = PlayerPrefs.GetFloat(SFXPref);
                settings[(int)Settings.TXTCLR] = PlayerPrefs.GetFloat(TextColor);
                if (settings[(int)Settings.TXTCLR] == -1)
                {
                    textColor = false;
                }
                else
                {
                    textColor = true;
                }
                settings[(int)Settings.RUMB] = PlayerPrefs.GetFloat(Rumble);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NewGame()
    {
        DataManager.gameData = new GameData();//remove this to make it only a new settings button?
        textColor = false;
        PlayerPrefs.SetFloat(Sensitivity, .5f);
        PlayerPrefs.SetFloat(MasPref, .5f);
        PlayerPrefs.SetFloat(BGMPref, 1);
        PlayerPrefs.SetFloat(SFXPref, 1);
        PlayerPrefs.SetFloat(TextColor, -1);
        PlayerPrefs.SetFloat(Rumble, 1);
        PlayerPrefs.SetString("Rebinds", "");
        GetAllInputs();
        //GameManager.Instance.GetComponent<PlayerInput>().actions.RemoveAllBindingOverrides();
        //GetInputs();
        settings[(int)Settings.SENSE] = PlayerPrefs.GetFloat(Sensitivity);
        settings[(int)Settings.MAS] = PlayerPrefs.GetFloat(MasPref);
        settings[(int)Settings.BGM] = PlayerPrefs.GetFloat(BGMPref);
        settings[(int)Settings.SFX] = PlayerPrefs.GetFloat(SFXPref);
        settings[(int)Settings.TXTCLR] = PlayerPrefs.GetFloat(TextColor);
        settings[(int)Settings.RUMB] = PlayerPrefs.GetFloat(Rumble);
        PlayerPrefs.SetFloat(Firstplay, -1);
        //PlayerPrefs.SetInt(Depr, 0);
        //PlayerPrefs.SetInt(Barg, 0);
        //PlayerPrefs.SetInt(Anger, 0);
    }

    // Is game using a cursor controlled by keyboard?
    public bool GetMouseCursor()
    {
        return menuCursor;
    }

    // Return a pref value
    public float GetFloat(Settings value)
    {
        return (float)settings[(int)value];
    }

    public void SetFloat(Settings index, float value)
    {
        settings[(int)index] = value;
        PlayerPrefs.SetFloat(statics[(int)index], value);
    }

    // Level completion data (move to DataManager?)
    public void CompleteLevel(Levels level)
    {
        DataManager.gameData.levelCompletion[(int)level] = true;
        //PlayerPrefs.SetInt(levelNames[(int)level], 1);
    }

    public bool GetLevelStatus(Levels level)
    {
        return DataManager.gameData.levelCompletion[(int)level];
        //return PlayerPrefs.GetInt(levelNames[(int)level]) == 1;
    }

    // Toggles ghost mode at player position while freezing time (maybe have an oldTimeScale variable if it becomes a problem)
    public void ToggleGhost()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if(player == null)
                return;
        }
        ghost = !ghost;
        if(ghost)
        {
            Time.timeScale = 0;
            BlockPlayerInput();
            spawnedGhost = Instantiate(ghostObj, player.transform.position, player.transform.rotation);
        }
        else
        {
            Time.timeScale = 1;
            EnablePlayerInput();
            Destroy(spawnedGhost);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // Globally available function to allow and stop player inputs regardless of any scripts
    public void EnablePlayerInput()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
                return;
        }
        player.GetComponent<BasicMovement>().playerControls.Enable();
        player.GetComponent<AbilityPush>().pushControls.Enable();
        player.GetComponent<Grapple>().grappleControls.Enable();
        player.GetComponent<SummonClone>().summonControls.Enable();
        player.GetComponent<PlayerInteractions>().playerControls.Enable();
        GameObject clone = GameObject.FindGameObjectWithTag("Clone");
        if (clone != null)
        {
            clone.GetComponent<BasicMovement>().playerControls.Enable();
            clone.GetComponent<AbilityPush>().pushControls.Enable();
            clone.GetComponent<Grapple>().grappleControls.Enable();
            clone.GetComponent<CloneInteractions>().playerControls.Enable();
        }
        GameObject pause = GameObject.FindGameObjectWithTag("Pause");
        if (pause != null)
        {
            pause.GetComponent<MenuTraverse>().playerControls.Enable();
        }
        GameObject pauseController = GameObject.FindGameObjectWithTag("PauseController");
        if (pauseController != null)
        {
            pauseController.GetComponent<PauseMenu>().playerControls.Enable();
        }
    }

    public void BlockPlayerInput()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
                return;
        }
        player.GetComponent<BasicMovement>().playerControls.Disable();
        player.GetComponent<AbilityPush>().pushControls.Disable();
        player.GetComponent<Grapple>().grappleControls.Disable();
        player.GetComponent<SummonClone>().summonControls.Disable();
        player.GetComponent<PlayerInteractions>().playerControls.Disable();
        GameObject clone = GameObject.FindGameObjectWithTag("Clone");
        if (clone != null)
        {
            clone.GetComponent<BasicMovement>().playerControls.Disable();
            clone.GetComponent<AbilityPush>().pushControls.Disable();
            clone.GetComponent<Grapple>().grappleControls.Disable();
            clone.GetComponent<CloneInteractions>().playerControls.Disable();
        }
        GameObject pause = GameObject.FindGameObjectWithTag("Pause");
        if(pause != null)
        {
            pause.GetComponent<MenuTraverse>().playerControls.Disable();
        }
        GameObject pauseController = GameObject.FindGameObjectWithTag("PauseController");
        if (pauseController != null)
        {
            pauseController.GetComponent<PauseMenu>().playerControls.Disable();
        }
    }

    public static void SelectControlScheme(string value)
    {
        GameManager.Instance.controlScheme = value;
    }

    public void ShowPuzzleWrapper(int puzzleID, float waitTime)
    {
        StartCoroutine(ShowPuzzle(puzzleID, waitTime));
    }
    
    public IEnumerator ShowPuzzle(int puzzleID, float waitTime)
    {
        DisableInput();
        
        GameObject mainCam = GameObject.FindGameObjectWithTag("StateDrivenCam");

        int previousCamState = mainCam.GetComponent<Animator>()
            .GetInteger("roomNum");
        
        mainCam.GetComponent<Animator>().SetInteger("roomNum", puzzleID);
        
        Debug.Log("before puzzle wait");
        yield return new WaitForSeconds(waitTime + 0.5f);
        Debug.Log("after puzzle wait");
        
        mainCam.GetComponent<Animator>().SetInteger("roomNum", previousCamState);
        
        EnableInput();
    }
    
    public void DisableInput()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        player.GetComponent<BasicMovement>().playerControls.Disable();
        player.GetComponent<AbilityPush>().pushControls.Disable();
        player.GetComponent<Grapple>().grappleControls.Disable();
        player.GetComponent<SummonClone>().summonControls.Disable();
        player.GetComponent<PlayerInteractions>().playerControls.Disable();
        player.GetComponentInChildren<LimitedMovementCam>().playerControls.Disable();

        if (GameObject.FindGameObjectWithTag("Clone") != null)
        {
            GameObject clone = GameObject.FindGameObjectWithTag("Clone");
            
            clone.GetComponent<BasicMovement>().playerControls.Disable();
            clone.GetComponent<AbilityPush>().pushControls.Disable();
            clone.GetComponent<Grapple>().grappleControls.Disable();
            clone.GetComponent<ExitClone>().summonControls.Disable();
            clone.GetComponent<CloneInteractions>().playerControls.Disable();
        }
    }

    public void EnableInput()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        player.GetComponent<BasicMovement>().playerControls.Enable();
        player.GetComponent<AbilityPush>().pushControls.Enable();
        player.GetComponent<Grapple>().grappleControls.Enable();
        player.GetComponent<SummonClone>().summonControls.Enable();
        player.GetComponent<PlayerInteractions>().playerControls.Enable();
        player.GetComponentInChildren<LimitedMovementCam>().playerControls.Enable();

        if (GameObject.FindGameObjectWithTag("Clone") != null)
        {
            GameObject clone = GameObject.FindGameObjectWithTag("Clone");
            
            clone.GetComponent<BasicMovement>().playerControls.Enable();
            clone.GetComponent<AbilityPush>().pushControls.Enable();
            clone.GetComponent<Grapple>().grappleControls.Enable();
            clone.GetComponent<ExitClone>().summonControls.Enable();
            clone.GetComponent<CloneInteractions>().playerControls.Enable();
        }
    }
}

public interface IAbility
{
    public void ResetRebind();
}

public enum Settings
{
    FIRSTPLAY = 0,
    BGM = 1,
    SFX = 2,
    MAS = 3,
    SENSE = 4,
    TXTCLR = 5,
    RUMB = 6
}

public enum Levels
{
    NA = 0,
    DEN = 1,
    HUB = 2,
    DEP = 3,
    BAR = 4,
    ANG = 5,

}