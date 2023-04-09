using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ErisAttackController : MonoBehaviour
{
    public Rigidbody playerRB;
    public Transform playerTrans;

    public ErisAttackStairs attackPrefab;

    public bool canAttack = false;
    float velocity = -4.0f;

    public bool hit = false;

    public GameObject deathScreen, continueButton, menuButton;

    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(InitalPause());
    }

    void FixedUpdate()
    {
        if (canAttack)
        {
            ErisAttackStairs attackSpawned = Instantiate(attackPrefab, 
            new Vector3(playerTrans.position.x, playerTrans.position.y + 10.0f, playerTrans.position.z), 
            Quaternion.identity);
            
            attackSpawned.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, velocity, 0.0f);
            attackSpawned.controller = this;

            StartCoroutine(AttackCooldown());
        }

        if (hit)
        {
            canAttack = false;

            StartCoroutine(FadeThenDie());
            GameObject.FindObjectOfType<PauseMenu>().DisableInput();
        }
    }

    private IEnumerator InitalPause()
    {
        yield return new WaitForSeconds(5.0f);

        canAttack = true;
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;

        yield return new WaitForSeconds(8.0f);

        if (!hit)
        {
            canAttack = true;
        }
    }

    public void FadetoBlack()
    {
        StartCoroutine(FadeThenDie());
    }

    private IEnumerator FadeThenDie()
    {
        if (GameObject.FindObjectOfType<FadeBlack>() != null)
        {
            GameObject.FindObjectOfType<FadeBlack>().FadeToBlack(1.5f);
        }

        yield return new WaitForSeconds(1.5f);
        
        deathScreen.SetActive(true);
        continueButton.SetActive(true);
        menuButton.SetActive(true);
    }
}
