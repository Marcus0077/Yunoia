using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ErisAttackController : MonoBehaviour
{
    public Rigidbody playerRB;
    public Transform playerTrans;
    public Transform erisTrans;
    public Transform target;

    public ErisAttack attackPrefab;

    public bool canAttack = false;
    float fallVelocity = -8.0f;
    float attackSpeed = 10.0f;

    public bool hit = false;
    public bool inBossRoom = false;
    public bool attackFrozen = false;

    public GameObject particles;

    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(InitalPause());
    }

    void FixedUpdate()
    {
        if (!inBossRoom)
        {
            if (canAttack)
            {
                if (!attackFrozen)
                {
                    ErisAttack attackSpawned = Instantiate(attackPrefab,
                        new Vector3(playerTrans.position.x, playerTrans.position.y + 20.0f, playerTrans.position.z),
                        Quaternion.identity);
                    Instantiate(particles,
                        new Vector3(playerTrans.position.x, playerTrans.position.y + 20.0f, playerTrans.position.z),
                        particles.transform.rotation);
                    attackSpawned.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, fallVelocity, 0.0f);
                    attackSpawned.controller = this;
                }

                StartCoroutine(AttackCooldown());
            }
        }
        else if (inBossRoom)
        {
            if (canAttack)
            {
                target = playerTrans;

                if (!attackFrozen)
                {
                    ErisAttack attackSpawned = Instantiate(attackPrefab,
                        new Vector3(erisTrans.position.x, erisTrans.position.y, erisTrans.position.z),
                        Quaternion.identity);
                    attackSpawned.transform.LookAt(target);
                    GameObject particleAttack = Instantiate(particles,
                        new Vector3(erisTrans.position.x, erisTrans.position.y, erisTrans.position.z),
                        particles.transform.rotation);
                    particleAttack.transform.LookAt(target);
                    attackSpawned.GetComponent<Rigidbody>().velocity = attackSpawned.transform.forward * attackSpeed;
                    attackSpawned.controller = this;
                }

                StartCoroutine(AttackCooldown());
            }
        }

        if (hit)
        {
            canAttack = false;

            //StartCoroutine(FadeThenDie());
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

        yield return new WaitForSeconds(4.0f);

        if (!hit)
        {
            canAttack = true;
        }
    }

    //public void FadetoBlack()
    //{
    //    StartCoroutine(FadeThenDie());
    //}

    //private IEnumerator FadeThenDie()
    //{
    //    GameManager.Instance.dying = true;
    //    if (GameObject.FindObjectOfType<FadeBlack>() != null)
    //    {
    //        GameObject.FindObjectOfType<FadeBlack>().FadeToBlack(1.5f);
    //    }

    //    yield return new WaitForSeconds(1.5f);

    //    GameObject.FindWithTag("MainCanvas").transform.Find("Lose Screen Object").gameObject.SetActive(true);
    //    GameManager.Instance.dying = false;
    //}
}
