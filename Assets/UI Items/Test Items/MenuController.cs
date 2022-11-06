using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class MenuController : MonoBehaviour
{
    private UIDocument doc;
    private Button playButton, settingsButton, exitButton, soundButton, backButton;
    public AudioSource soundSource;
    [SerializeField] private Sprite soundSprite;
    [SerializeField] private VisualTreeAsset settingsButtonsTemplate;
    [SerializeField] private VisualElement settingsButtons, buttonsWrapper;

    void Awake()
    {
        doc = GetComponent<UIDocument>();

        playButton = doc.rootVisualElement.Q<Button>("PlayButton");
        playButton.clicked += PlayButtonOnClicked;

        settingsButton = doc.rootVisualElement.Q<Button>("SettingsButton");
        settingsButton.clicked += SettingsButtonOnClicked;
        settingsButtons = settingsButtonsTemplate.CloneTree();
        var backButton = settingsButtons.Q<Button>("Button4");
        backButton.clicked += BackButtonOnClicked;

        exitButton = doc.rootVisualElement.Q<Button>("ExitButton");
        exitButton.clicked += ExitButtonOnClicked;

        soundButton = doc.rootVisualElement.Q<Button>("SoundButton");
        soundButton.clicked += SoundButtonOnClicked;

        buttonsWrapper = doc.rootVisualElement.Q<VisualElement>("Buttons");
    }

    private void PlayButtonOnClicked()
    {
        Debug.Log ("WOW IT WORKED!");
    }

    private void ExitButtonOnClicked()
    {
        Debug.Log("GAME QUIT!");
    }

    private void SoundButtonOnClicked()
    {
        soundSource.Play();
        var bg = soundButton.style.backgroundImage;
        bg.value = Background.FromSprite(soundSprite);
        soundButton.style.backgroundImage = bg;
    }

    private void SettingsButtonOnClicked()
    {
        buttonsWrapper.Clear();
        
        buttonsWrapper.Add(settingsButtons);
    }

    private void BackButtonOnClicked()
    {
        buttonsWrapper.Clear();
        buttonsWrapper.Add(playButton);
        buttonsWrapper.Add(settingsButton);
        buttonsWrapper.Add(exitButton);
    }
}
