using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivityController : MonoBehaviour
{
    [SerializeField]
    Settings prefName;

    void Start()
    {
        GetComponent<Slider>().value = GameManager.Instance.GetFloat(prefName);
    }

    public void OnChangeSlider(float value)
    {
        GameManager.Instance.SetFloat(prefName, value);
        PlayerPrefs.Save();
    }

}
