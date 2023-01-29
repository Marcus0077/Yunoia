using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivityController : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.sensitivity = PlayerPrefs.GetFloat("Sensitivity");
        GetComponent<Slider>().value = PlayerPrefs.GetFloat("Sensitivity");
    }

    public void OnChangeSlider(float Value)
    {
        PlayerPrefs.SetFloat("Sensitivity", Value);
        GameManager.Instance.sensitivity = PlayerPrefs.GetFloat("Sensitivity");
        PlayerPrefs.Save();
    }

}
