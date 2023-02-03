using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;

public class NewSettingsController : MonoBehaviour
{
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private AudioSource[] Source;
    [SerializeField] private AudioMixMode MixMode;

    private static readonly string SliderPref = "Volume";

    void Start()
    {
        //Mixer.SetFloat("Volume", Mathf.Log10(PlayerPrefs.GetFloat("Volume") * 20));
        Mixer.SetFloat("Volume", PlayerPrefs.GetFloat(SliderPref));
    }

    public void OnChangeSlider(float Value)
    {
        switch(MixMode)
        {
            case AudioMixMode.LinearMixerVolume:
                Mixer.SetFloat("Volume", (-80 + Value * 100));
                break;
            case AudioMixMode.LogrithmicMixerVolume:
                Mixer.SetFloat("Volume", Mathf.Log10(Value) * 20);
                break;
        }
    }


    public enum AudioMixMode
    {
        LinearAudioSourceVolume,
        LinearMixerVolume,
        LogrithmicMixerVolume
    }
}
