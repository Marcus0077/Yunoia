using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public class TeleToBeaver : MonoBehaviour
{
    public Transform telePoint;

    public GameObject player;
    private GameManager gameManager;
    private FadeBlack blackScreen;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.FindObjectOfType<GameManager>();
        blackScreen = FindObjectOfType<FadeBlack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GameObject();

            StartCoroutine(FadeInOutBlack(1.5f));
        }
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
        
        player.transform.position = telePoint.transform.position;
        
        yield return new WaitForSeconds(waitTime);

        blackScreen.FadeToTransparent(waitTime);

        gameManager.EnableInput();
    }
}
