using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuTransition : MonoBehaviour
{
    public bool inSettings;
    public float xSize;
    public float ySize;
    public CanvasGroup bg;
    public GameObject prevMenu;

    public void Awake()
    {
        inSettings = false;
        bg.alpha = 0;
        transform.localScale = Vector2.zero;
    }

    public void Open()
    {
        transform.LeanScale(new Vector2(xSize, ySize), 0.25f).setIgnoreTimeScale(true);
        gameObject.SetActive(true);
        bg.LeanAlpha(1, 1f).setEaseOutExpo().setIgnoreTimeScale(true);

        inSettings = true;
    }

    public void Close()
    {
        transform.LeanScale(Vector2.zero, 0.8f).setEaseInBack().setIgnoreTimeScale(true);
        bg.LeanAlpha(0, 0.5f).setIgnoreTimeScale(true);
        StartCoroutine(NextMenu(0.8f));

        inSettings = false;
    }

    private IEnumerator NextMenu(float time)
    {
        yield return new WaitForSeconds(time);
        prevMenu.SetActive(true);
    }
}
