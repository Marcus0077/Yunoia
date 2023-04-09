using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform teleportTarget;
    public GameObject player, terrain, ravine;

    private GameManager gameManager;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager
    }

    void OnTriggerEnter (Collider collider)
    {
        
    }

    private IEnumerator FadeInOut()
    {
        
        player.transform.position = teleportTarget.transform.position;

        if (ravine != null && terrain != null)
        {
            ravine.SetActive(false);
            terrain.SetActive(true);
        }
        
        LeanTween.color(GetComponent<RectTransform>(), Color.clear, 3f);
    }
}
