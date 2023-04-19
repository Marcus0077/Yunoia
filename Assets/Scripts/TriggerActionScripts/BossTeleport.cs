using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleport : MonoBehaviour
{
    public Transform teleportTarget;
    public GameObject player;

    public ErisAttackController controller;

    private GameManager gameManager;
    private FadeBlack blackScreen;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.FindObjectOfType<GameManager>();
        blackScreen = FindObjectOfType<FadeBlack>();
    }

    public void OnTriggerEnter(Collider collider)
    {
        StartCoroutine(FadeInOutBlack(1.5f));
    }
    
    public IEnumerator FadeInOutBlack(float waitTime)
    {
        gameManager.DisableInput();
        controller.attackFrozen = true;
            
        blackScreen.FadeToBlack(waitTime);

        yield return new WaitForSeconds(waitTime);
        
        if (GameObject.FindWithTag("Clone") != null)
        {
            GameObject.FindWithTag("Clone").GetComponent<ExitClone>().despawnClone = true;
        }
        
        player.transform.position = teleportTarget.transform.position;
        controller.StopAllCoroutines();

        yield return new WaitForSeconds(waitTime);
        
        blackScreen.FadeToTransparent(waitTime);

        controller.inBossRoom = true;
        gameManager.EnableInput();
        
        yield return new WaitForSeconds(waitTime);

        controller.attackFrozen = false;
        controller.canAttack = true;
    }
}
