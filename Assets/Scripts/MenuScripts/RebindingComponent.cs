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
    GameObject player, clone;
    AbilityPush push;
    BasicMovement movement;
    SummonClone cloner;
    Grapple grapple;
    TextMeshProUGUI displayText;
    InputActionRebindingExtensions.RebindingOperation rebindOperation;
    string binding;
    int bindingIndex;
    InputAction[] actions;
    InputActionAsset action;
    // Start is called before the first frame update
    void OnEnable()
    {
        actions = null;
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        if (clone == null)
            clone = GameObject.FindGameObjectWithTag("Clone");
        displayText = GetComponent<TextMeshProUGUI>();
        switch ((int)type)
        {
            case 0:
                actions = new InputAction[1];
                push = player.GetComponent<AbilityPush>();
                displayText.text = "Push Ability: ";
                actions[0] = push.pushAction;
                action = push.pushControls;
                break;
            case 1:
                cloner = player.GetComponent<SummonClone>();
                displayText.text = "Clone Ability: ";
                if (clone == null)
                {
                    actions = new InputAction[3];
                    actions[0] = cloner.summonAClone;
                    actions[1] = cloner.switchPlaces;
                    actions[2] = cloner.exitClone;
                }
                else
                {
                    actions = new InputAction[3];
                    actions[0] = cloner.summonAClone;
                    actions[1] = cloner.switchPlaces;
                    actions[2] = cloner.exitClone;
                    //actions[3] = clone.GetComponent<CloneInteractions>().switchPlaces;
                    //actions[4] = clone.GetComponent<ExitClone>().exitClone;
                }
                action = cloner.summonControls;
                //need a way to change clone controls. (have exitclone and cloneinteractions copy summonclone action?)
                break;
            case 2:
                actions = new InputAction[1];
                grapple = player.GetComponent<Grapple>();
                displayText.text = "Grapple Ability: ";
                actions[0] = grapple.shootHook;
                action = grapple.grappleControls;
                break;
            case 3:
                actions = new InputAction[1];
                movement = player.GetComponent<BasicMovement>();
                displayText.text = "Dash Ability: ";
                actions[0] = movement.dash;
                action = movement.playerControls;
                break;
        }
        bindingIndex = actions[0].GetBindingIndex(group: GameManager.Instance.controlScheme);
        actions[0].GetBindingDisplayString(bindingIndex, out string device, out binding);
        displayText.text += binding;
    }

    public void RemapButtonClicked()
    {
        dimmer.alpha = 1;
        if(rebindOperation != null)
        {
            rebindOperation.Dispose();
        }
        rebindOperation = actions[0].PerformInteractiveRebinding()
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
            if (bindings.path == actions[0].bindings[0].path.ToString())
                continue;
            rebindOperation.WithControlsExcluding(bindings.path);
        }
        rebindOperation.Start();
    }

    void RebindCompletion()
    {
        for(int i = 1; i < actions.Length; i++)
        {
            actions[i].ApplyBindingOverride(actions[0].bindings[0].overridePath, group : GameManager.Instance.controlScheme);
        }
        var rebinds = action.SaveBindingOverridesAsJson();
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