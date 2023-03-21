using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RebindingComponent : MonoBehaviour
{
    [SerializeField]
    Controls type;
    [SerializeField]
    CanvasGroup dimmer;
    GameObject player;
    AbilityPush push;
    BasicMovement movement;
    SummonClone cloner;
    Grapple grapple;
    TextMeshProUGUI displayText;
    InputActionRebindingExtensions.RebindingOperation rebindOperation;
    string binding;
    InputAction[] action;
    // Start is called before the first frame update
    void OnEnable()
    {
        action = null;
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        displayText = GetComponent<TextMeshProUGUI>();
        switch ((int)type)
        {
            case 0:
                action = new InputAction[1];
                push = player.GetComponent<AbilityPush>();
                displayText.text = "Push Ability: ";
                action[0] = push.pushAction;
                break;
            case 1:
                action = new InputAction[1];
                cloner = player.GetComponent<SummonClone>();
                displayText.text = "Clone Ability: ";
                action[0] = cloner.summonAClone;
                //need a way to change clone controls. (have exitclone and cloneinteractions copy summonclone action?)
                break;
            case 2:
                action = new InputAction[1];
                grapple = player.GetComponent<Grapple>();
                displayText.text = "Grapple Ability: ";
                action[0] = grapple.shootHook;
                break;
            case 3:
                action = new InputAction[1];
                movement = player.GetComponent<BasicMovement>();
                displayText.text = "Dash Ability: ";
                action[0] = movement.dash;
                break;
        }
        int bindingIndex = action[0].GetBindingIndex(group: GameManager.Instance.controlScheme);
        action[0].GetBindingDisplayString(bindingIndex, out string device, out binding);
        displayText.text += binding;
    }

    public void RemapButtonClicked()
    {
        dimmer.alpha = 1;
        if(rebindOperation != null)
        {
            rebindOperation.Dispose();
        }
        rebindOperation = action[0].PerformInteractiveRebinding()
            .WithBindingGroup(GameManager.Instance.controlScheme)
            .WithCancelingThrough("<Keyboard>/escape")
            .WithCancelingThrough("<Gamepad>/start")
            .OnMatchWaitForAnother(0.1f)
            .WithControlsExcluding("<Keyboard>/anyKey")
            .WithControlsExcluding("<Mouse>/press")
            .OnCancel(operation => dimmer.alpha = 0)
            .OnComplete(operation => RebindCompletion());
        foreach (var bindings in GameManager.Instance.GetComponent<PlayerInput>().actions.bindings)
        {
            if (bindings.path == action[0].bindings[0].path.ToString())
                continue;
            rebindOperation.WithControlsExcluding(bindings.path);
        }
        rebindOperation.Start();
    }

    void RebindCompletion()
    {
        var rebinds = action[0].SaveBindingOverridesAsJson();
        Debug.Log(rebinds);
        PlayerPrefs.SetString("Rebinds", rebinds);
        OnEnable();
        dimmer.alpha = 0;
        rebindOperation.Dispose();
    }
}

public enum Controls
{
    PUSH = 0,
    CLONE = 1,
    GRAPPLE = 2,
    DASH = 3
}