using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ErisBoss : MonoBehaviour
{
    public GameObject damageShield;
    public GameObject bossShield;
    public Transform player;
    public GameObject fallEris;
    public float health;

    private GameObject EndScene;

    // Start is called before the first frame update
    void Awake()
    {
        health = 3.0f;
        EndScene = GameObject.FindGameObjectWithTag("EndScene");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0 && !damageShield.activeInHierarchy)
        {
            EndScene.SetActive(true);
            bossShield.SetActive(false);

            GameManager.Instance.StartEndCutscene(gameObject, fallEris);
        }
        else
        {
            EndScene.SetActive(false);
        }

        this.transform.LookAt(player);
    }

    public void ErisHurt()
    {
        health -= 1.0f;

        StartCoroutine(DamageShield());
    }

    private IEnumerator DamageShield()
    {
        damageShield.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        damageShield.SetActive(false);
    }
}
