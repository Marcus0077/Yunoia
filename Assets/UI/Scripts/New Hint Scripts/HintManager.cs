using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HintManager : MonoBehaviour
{
    public TextMeshProUGUI hintText;
    public bool NoHints = false;

    Hint[] currentHint;
    int activeMessage = 0;

    public void OpenHint(Hint[] hints)
    {
        currentHint = hints;
        activeMessage = 0;

        Debug.Log("Hint Displayed! Loaded hints: " + hints.Length);
        DisplayHint();
    }

    void DisplayHint()
    {
        Hint hintToDisplay = currentHint[activeMessage];
        hintText.text = hintToDisplay.hint;
        NoHints = false;
    }

    public void NextHint()
    {
        activeMessage++;
        if(activeMessage < currentHint.Length)
        {
            DisplayHint();
        }
        else
        {
            Debug.Log("No More Hints.");
            CloseHint();
        }
    }

    public void CloseHint()
    {
        NoHints = true;
    }
}
