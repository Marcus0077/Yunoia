using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RebindingComponent : MonoBehaviour
{
    [SerializeField]
    Controls type;
    GameObject player;
    AbilityPush push;
    BasicMovement movement;
    SummonClone cloner;
    Grapple grapple;
    TextMeshProUGUI displayText;
    string binding;
    InputAction action;
    // Start is called before the first frame update
    void OnEnable()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        displayText = GetComponent<TextMeshProUGUI>();
        switch ((int)type)
        {
            case 0:
                push = player.GetComponent<AbilityPush>();
                displayText.text = "Push Ability: ";
                action = push.pushAction;
                break;
            case 1:
                cloner = player.GetComponent<SummonClone>();
                displayText.text = "Clone Ability: ";
                break;
            case 2:
                grapple = player.GetComponent<Grapple>();
                displayText.text = "Grapple Ability: ";
                break;
        }
        int bindingIndex = action.GetBindingIndex(group: player.GetComponent<PlayerInput>().currentControlScheme);
        action.GetBindingDisplayString(bindingIndex, out string device, out binding);
        displayText.text += binding;
    }

    public void RemapButtonClicked()
    {
        var rebindOperation = action.PerformInteractiveRebinding()
                    // To avoid accidental input from mouse motion
                    .WithControlsExcluding("Mouse")
                    .OnMatchWaitForAnother(0.1f)
                    .Start();
    }
}

public enum Controls
{
    PUSH = 0,
    CLONE = 1,
    GRAPPLE = 2
}