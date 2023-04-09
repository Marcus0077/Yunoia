using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform teleportTarget;
    public GameObject player, terrain, ravine;

    private GameManager gameManager;
    private FadeBlack blackScreen;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.FindObjectOfType<GameManager>();
        blackScreen = FindObjectOfType<FadeBlack>();
    }

    void OnTriggerEnter (Collider collider)
    {
        StartCoroutine(FadeInOutBlack(1.5f));
    }

    public IEnumerator FadeInOutBlack(float waitTime)
    {
        gameManager.DisableInput();

        if (GameObject.FindGameObjectWithTag("Clone") != null)
        {
            GameObject clone = GameObject.FindGameObjectWithTag("Clone");
            clone.GetComponent<ExitClone>().Timer += waitTime;
        }
            
        blackScreen.FadeToBlack(waitTime);

        yield return new WaitForSeconds(waitTime);
        
        player.transform.position = teleportTarget.transform.position;
        
        yield return new WaitForSeconds(waitTime);

        if (ravine != null && terrain != null)
        {
            ravine.SetActive(false);
            terrain.SetActive(true);
        }
        
        blackScreen.FadeToTransparent(waitTime);

        gameManager.EnableInput();
    }
}
