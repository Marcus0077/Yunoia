using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Strings for settings and stage names
    private static readonly string Firstplay = "First Play";
    private static readonly string BGMPref = "BGM Pref";
    private static readonly string SFXPref = "SFX Pref";
    private static readonly string MasPref = "Mas Pref";
    private static readonly string Sensitivity = "Sensitivity";
    private static readonly string Depr = "Depression";
    private static readonly string Barg = "Barg";
    private static readonly string Anger = "Anger";
    private static readonly string Denial = "Denial";
    private static readonly string TextColor = "TextColor";

    public string[] statics = { Firstplay,BGMPref,SFXPref,MasPref,Sensitivity,TextColor };
    public string[] levelNames = { Depr, Barg, Anger, Denial };
    public double[] settings = new double[System.Enum.GetValues(typeof(Settings)).Length];

    public static GameManager instance;
    public bool ghost = false, menuCursor = false, textColor = false;
    [SerializeField]
    GameObject player, ghostObj;
    GameObject spawnedGhost;
    List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
    public float time;
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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!textColor)
            return;
        texts = new List<TextMeshProUGUI>(FindObjectsOfType<TextMeshProUGUI>());
        texts.ForEach(text => text.color = ConvertFloatToHex(settings[(int)Settings.TXTCLR]));
    }

    public void SetColor()
    {
        if (!textColor)
            return;
        texts = new List<TextMeshProUGUI>(FindObjectsOfType<TextMeshProUGUI>());
        texts.ForEach(text => text.color = ConvertFloatToHex(settings[(int)Settings.TXTCLR]));
    }

    double ConvertHexToFloat(Color color)
    {
        return double.Parse(color.r.ToString("000") + color.g.ToString("000") + color.b.ToString("000") + color.a.ToString("000"));
    }

    Color ConvertFloatToHex(double value)
    {
        string colorString = value.ToString("000000000000");
        Debug.Log(colorString);
        return new Color(float.Parse(colorString.Substring(0, 3)), float.Parse(colorString.Substring(3, 3)), float.Parse(colorString.Substring(6, 3)), float.Parse(colorString.Substring(9, 3)));
    }

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        player = GameObject.FindGameObjectWithTag("Player");
        SceneManager.sceneLoaded += OnSceneLoaded;
        // Set variables ready for a new game (create a button that sets Firstplay to 0 for new game?)
        if (PlayerPrefs.GetFloat(Firstplay) == 0)
        {
            PlayerPrefs.SetFloat(Sensitivity,.5f);
            PlayerPrefs.SetFloat(MasPref,.5f);
            PlayerPrefs.SetFloat(BGMPref,1);
            PlayerPrefs.SetFloat(SFXPref,1);
            PlayerPrefs.SetFloat(TextColor, 255255255255);
            settings[(int)Settings.SENSE] = PlayerPrefs.GetFloat(Sensitivity);
            settings[(int)Settings.MAS] = PlayerPrefs.GetFloat(MasPref);
            settings[(int)Settings.BGM] = PlayerPrefs.GetFloat(BGMPref);
            settings[(int)Settings.SFX] = PlayerPrefs.GetFloat(SFXPref);
            settings[(int)Settings.TXTCLR] = PlayerPrefs.GetFloat(TextColor);
            PlayerPrefs.SetFloat(Firstplay, -1);
            PlayerPrefs.SetInt(Depr, 0);
            PlayerPrefs.SetInt(Barg, 0);
            PlayerPrefs.SetInt(Anger, 0);
        }
        else
        {
            settings[(int)Settings.SENSE] = PlayerPrefs.GetFloat(Sensitivity);
            settings[(int)Settings.MAS] = PlayerPrefs.GetFloat(MasPref);
            settings[(int)Settings.BGM] = PlayerPrefs.GetFloat(BGMPref);
            settings[(int)Settings.SFX] = PlayerPrefs.GetFloat(SFXPref);
            settings[(int)Settings.TXTCLR] = PlayerPrefs.GetFloat(TextColor);
        }
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

    public void CompleteLevel(Levels level)
    {
        PlayerPrefs.SetInt(levelNames[(int)level], 1);
    }

    public bool GetLevelStatus(Levels level)
    {
        return PlayerPrefs.GetInt(levelNames[(int)level]) == 1;
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

    void OnEnabled()
    {
        
    }

    void OnDisable()
    {
        
    }

    void FixedUpdate()
    {
        
    }
}

public enum Settings
{
    FIRSTPLAY = 0,
    BGM = 1,
    SFX = 2,
    MAS = 3,
    SENSE = 4,
    TXTCLR = 5
}

public enum Levels
{
    DEP = 0,
    BAR = 1,
    ANG = 2,
    DEN = 3
}