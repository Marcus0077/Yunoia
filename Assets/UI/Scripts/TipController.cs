using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip butterflyrisAwakeSound;
    public Transform box;

    public CanvasGroup background;

    public void OnEnable()
    {
        background.alpha = 0;
        background.LeanAlpha(1, 0.5f);

        box.localPosition = new Vector2(0, -Screen.height);
        box.LeanMoveLocalY(-150, 0.5f).setEaseOutExpo().delay = 0.1f;
        audioSource.PlayOneShot(butterflyrisAwakeSound);
    }

    public void CloseDialog()
    {
        background.LeanAlpha(0, 0.5f);
        box.LeanMoveLocalY(-Screen.height, 0.5f).setEaseInExpo().setOnComplete(OnComplete);
        Debug.Log("Closed");
    }

    public void OnComplete()
    {
        //gameObject.SetActive(false);
    }
}