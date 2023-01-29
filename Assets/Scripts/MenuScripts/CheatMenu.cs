using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class CheatMenu : MonoBehaviour
{
    [SerializeField]
    TMP_Text historyText;
    [SerializeField]
    TMP_InputField inputField;
    [SerializeField]
    ScrollRect scroller;
    PlayerControls playerControls;
    InputAction toggle;
    InputAction history;
    CanvasGroup cg;
    string commands, returnValue;
    string greenText = "<color=#00FF00>", colorEnd = "</color>";
    List<string> clipboard = new List<string>();
    int selected = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindObjectsOfType(typeof(CheatMenu),true).ToArray().Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        cg = GetComponent<CanvasGroup>();
        Deselected();
    }

    public void TextInput(string text)
    {
        commands = text; 
        DoCommand();
        clipboard.Add(commands);
        selected = clipboard.Count;
    }

    public void Selected()
    {
        cg.alpha = .75f;
        cg.interactable = true;
        inputField.ActivateInputField();
    }

    public void Deselected()
    {
        cg.alpha = 0f;
        cg.interactable = false;
        inputField.DeactivateInputField();
    }

    public void DoCommand()
    {
        //check command value
        if(commands != "" && commands.Substring(0, Mathf.Min(commands.Length,3)) == "go ")
        {
            int failed;
            string place = commands.Substring(3, commands.Length - 3);
            if (SceneUtility.GetBuildIndexByScenePath(place) != -1)
            {
                scroller.verticalNormalizedPosition = 0;
                UpdateHistory(commands);
                UpdateHistory(greenText + "went to " + commands.Substring(3, commands.Length - 3) + colorEnd);
                SceneManager.LoadScene(place);
                Toggle();
            }
            else if (int.TryParse(place,out failed) && SceneUtility.GetScenePathByBuildIndex(int.Parse(place)) != "")
            {
                scroller.verticalNormalizedPosition = 0;
                UpdateHistory(commands);
                UpdateHistory(greenText + "went to scene index " + commands.Substring(3, commands.Length - 3) + colorEnd);
                SceneManager.LoadScene(int.Parse(place));
                Toggle();
            }
            else
            {
                scroller.verticalNormalizedPosition = 0;
                UpdateHistory(commands);
                UpdateHistory(greenText + "Enter valid scene (in build settings)" + colorEnd);
            }
        }
        else if(commands != "" && commands.Substring(0, Mathf.Min(commands.Length, 5)) == "ghost")
        {
            UpdateHistory(commands);
            if(GameManager.Instance.ghost)
            {
                UpdateHistory(greenText + "Ghost mode off" + colorEnd);
            }
            else
            {
                UpdateHistory(greenText + "Ghost mode on" + colorEnd);
            }
            Toggle();
            GameManager.Instance.ToggleGhost();
        }
        inputField.text = "";
        inputField.ActivateInputField();
    }

    public void UpdateHistory(string text)
    {
        historyText.text += text + "<br>";
    }

    public void Toggle()
    {
        cg.interactable = !cg.interactable;
        if (cg.interactable)
        {
            GameManager.Instance.BlockPlayerInput();
            cg.alpha = .75f;
            inputField.ActivateInputField();
        }
        else
        {
            GameManager.Instance.EnablePlayerInput();
            cg.alpha = 0f;
            inputField.DeactivateInputField();
        }
    }

    public void Clipboard(InputAction.CallbackContext ctx)
    {
        if (clipboard.Count != 0)
        {
            Debug.Log(Mathf.Max(0, Mathf.Min(selected - (int)ctx.ReadValue<float>(), clipboard.Count - 1)));
            selected = Mathf.Min(selected - (int)ctx.ReadValue<float>(), clipboard.Count - 1);
            if(selected < 0)
            {
                inputField.text = "";
                selected = clipboard.Count;
            } else
            {
                inputField.text = clipboard[selected];
            }
            
        }
    }

    void Awake()
    {
        playerControls = new PlayerControls();
        toggle = playerControls.Cheat.Toggle;
        toggle.performed += ctx => Toggle();
        history = playerControls.Cheat.History;
        history.performed += Clipboard;
    }
    
    private void OnEnable()
    {
        toggle.Enable();
        history.Enable();
    }

    private void OnDisable()
    {
        toggle.Disable();
        history.Disable();
    }
}
