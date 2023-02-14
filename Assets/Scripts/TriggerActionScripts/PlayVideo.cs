using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayVideo : MonoBehaviour
{
    // Input variables
    PlayerControls playerControls;
    public InputAction skipCutscene;

    public GameObject videoPlayer;

    private bool videoPlaying;

    public GameObject aToSkip;
    private bool canSkip;

    // Start is called before the first frame update
    void Start()
    {
        canSkip = false;
        aToSkip.SetActive(false);
        
        playerControls = new PlayerControls();
        skipCutscene = playerControls.Cutscene.SkipCutscene;
        skipCutscene.Enable();
        videoPlayer.SetActive(false);

        videoPlaying = false;
    }
    
    void Update()
    {
        if (skipCutscene.WasPressedThisFrame() && videoPlaying && canSkip)
        {
            StopCoroutine(PlayOutro());
            SceneManager.LoadScene("Main Menu");
        }
    }

    // Update is called once per frame
  void OnTriggerEnter (Collider player)
  { 
      if (player.gameObject.tag == "Player")
      {
          StartCoroutine(PlayOutro());
      }
  }

  private IEnumerator PlayOutro()
  {
      videoPlaying = true;
      
      if (GameObject.FindObjectOfType<FadeBlack>() != null && GameObject.FindObjectOfType<PauseMenu>() != null)
      {
          GameObject.FindObjectOfType<PauseMenu>().DisableInput();
          GameObject.FindObjectOfType<FadeBlack>().FadeToBlack();
          MusicPersist.Instance.GetComponent<AudioSource>().Stop();

          yield return new WaitForSeconds(1.5f);

          Destroy(GameObject.FindGameObjectWithTag("UIIcons"));
          GameObject.FindObjectOfType<FadeBlack>().FadeToTransparent();
          videoPlayer.SetActive(true);
          
          yield return new WaitForSeconds(3f);

          aToSkip.SetActive(true);
          canSkip = true;
          
          yield return new WaitForSeconds(37f);

          SceneManager.LoadScene("Main Menu");
      }
  }
}