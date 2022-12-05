using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayVideo : MonoBehaviour
{

    public GameObject videoPlayer;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.SetActive(false);
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
      if (GameObject.FindObjectOfType<FadeBlack>() != null && GameObject.FindObjectOfType<PauseMenu>() != null)
      {
          GameObject.FindObjectOfType<PauseMenu>().DisableInput();
          GameObject.FindObjectOfType<FadeBlack>().FadeToBlack();
          MusicPersist.Instance.GetComponent<AudioSource>().Stop();

          yield return new WaitForSeconds(1.5f);

          Destroy(GameObject.FindGameObjectWithTag("UIIcons"));
          GameObject.FindObjectOfType<FadeBlack>().FadeToTransparent();
          videoPlayer.SetActive(true);
          
          yield return new WaitForSeconds(40f);

          SceneManager.LoadScene("Main Menu");
      }
  }
}
