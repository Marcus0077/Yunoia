using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ErisBoss : MonoBehaviour
{
    public GameObject damageShield;
    public GameObject bossShield;
    public Transform player;
    float health;

    // Start is called before the first frame update
    void Awake()
    {
        health = 3.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0)
        {
            bossShield.SetActive(false);

            StartCoroutine(PlayEndCutscene());
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

    private IEnumerator PlayEndCutscene()
    {
        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene("CutsceneEnd");
    }
}
