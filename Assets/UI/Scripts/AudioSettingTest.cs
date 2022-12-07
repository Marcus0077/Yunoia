using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingTest : MonoBehaviour
{
    private static readonly string MasPref = "Mas Pref";
    private float  masFloat;
    private static readonly string SliderPref = "Volume";

    public Slider bgmSlider;


    void Start()
    {
        ContinueSettings(); 
        Debug.Log(PlayerPrefs.GetFloat(SliderPref));
        Debug.Log(bgmSlider.value.ToString());
    }

    private void ContinueSettings()
    {
        masFloat = PlayerPrefs.GetFloat(MasPref);

        AudioListener.volume = masFloat;

        bgmSlider.value = PlayerPrefs.GetFloat(SliderPref);
    }
}
