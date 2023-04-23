using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    private static readonly string Firstplay = "First Play";
    private static readonly string BGMPref = "BGM Pref";
    private static readonly string SFXPref = "SFX Pref";
    private static readonly string MasPref = "Mas Pref";

    private int firstPlayInt;
    public Slider masSlider; 
    private float masFloat;

   // public AudioListener masAudio;

    void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(Firstplay);
        if (firstPlayInt == 0)
        {
            masFloat = .50f;
            masSlider.value = masFloat;
            PlayerPrefs.SetFloat(MasPref, masFloat);
            PlayerPrefs.SetInt(Firstplay, -1);
        }
        else
        {
            masFloat = PlayerPrefs.GetFloat(MasPref);
            masSlider.value = masFloat;
        }
    }

    public void SaveSoundSettings()
    {
        //PlayerPrefs.SetFloat(MasPref, masSlider.value);
        PlayerPrefs.Save();
    }

    void OnApplicationFocus(bool inFocus)
    {
        if(!inFocus)
        {
            SaveSoundSettings();
        }
    }

    public void UpdateSound()
    {
        AudioListener.volume = masSlider.value;
    }
}
