using System;
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

    [SerializeField]
    Settings prefName;

    void Start()
    {
        //Mixer.SetFloat("Volume", Mathf.Log10(PlayerPrefs.GetFloat("Volume") * 20));
        switch (MixMode)
        {
            case AudioMixMode.LinearMixerVolume:
                Mixer.SetFloat("Volume", (-80 + GameManager.Instance.GetFloat(prefName) * 100));
                break;
            case AudioMixMode.LogrithmicMixerVolume:
                Mixer.SetFloat("Volume", Mathf.Log10(GameManager.Instance.GetFloat(prefName)) * 20);
                break;
        }
        if(Source.Length > 1)
        {
            List<AudioSource> sources = new List<AudioSource>(FindObjectsOfType<AudioSource>());
            AudioSource remove = new AudioSource();
            foreach (AudioSource audioSource in sources)
            {
                if (audioSource.gameObject.name == "GameManager" || audioSource.gameObject.name == "Main Camera") ;
                {
                    remove = audioSource;
                }
            }
            if(remove != null)
                sources.Remove(remove);
            Source = new AudioSource[sources.Count];
            for (int i = 0; i < sources.Count; i++)
            {
                Source[i] = sources[i];
            }
        }
        else
        {
            Source = new AudioSource[2];
            Source[0] = GameManager.Instance.GetComponent<AudioSource>();
            Source[1] = Camera.main.GetComponent<AudioSource>();
        }
        foreach (AudioSource audioSource in Source)
        {
            if(audioSource != null)
                audioSource.outputAudioMixerGroup = Mixer.FindMatchingGroups("Master")[0];
        }
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
