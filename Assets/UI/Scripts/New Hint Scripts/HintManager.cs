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
        StopAllCoroutines();
        StartCoroutine(TypeHint(hintToDisplay.hint));

        //AnimateTextColor();
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

    void AnimateTextColor()
    {
        LeanTween.textAlpha(hintText.rectTransform, 0, 0);
        LeanTween.textAlpha(hintText.rectTransform, 1, 0.5f);
        Debug.Log("Text faded");
    }

    IEnumerator TypeHint(string hint)
    {
        hintText.text = "";
        foreach (char letter in hint.ToCharArray())
        {
            hintText.text += letter;
            yield return null;
        }
    }
}
