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

    public static bool isOpen = false;

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
        isOpen = true;

        gameManager.DisableInput();
            
        blackScreen.FadeToBlack(waitTime);

        yield return new WaitForSeconds(waitTime);
        
        if (GameObject.FindWithTag("Clone") != null)
        {
            GameObject.FindWithTag("Clone").GetComponent<ExitClone>().despawnClone = true;
        }
        
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

    void Update()
    {
        if(isOpen == true)
        {
            terrain.SetActive(true);
        }
    }
}
