using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnButtonHover : MonoBehaviour
{
    [SerializeField] private GameObject startButton;
    public float xSize, ySize;

    public void OnHoverEnter()
    {
        LeanTween.color(startButton.GetComponent<RectTransform>(), Color.red, .5f).setEaseInOutQuint();
        transform.LeanScale(new Vector2(xSize, ySize), 0.25f).setEaseInOutQuint();
    }

    public void OnHoverExit()
    {
        LeanTween.color(startButton.GetComponent<RectTransform>(), Color.white, .5f).setEaseInOutQuint();
        transform.LeanScale(new Vector2(2, 2), 0.25f).setEaseInOutQuint();
    }
}
