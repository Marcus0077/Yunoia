using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuTransition : MonoBehaviour
{
    public float xSize;
    public float ySize;

    public void Awake()
    {
        transform.localScale = Vector2.zero;
    }

    public void Open()
    {
        transform.LeanScale(new Vector2(xSize, ySize), 0.25f);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        transform.LeanScale(Vector2.zero, 0.8f).setEaseInBack();
    }
}
