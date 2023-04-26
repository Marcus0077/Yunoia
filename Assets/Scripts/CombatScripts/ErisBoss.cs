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
    public Color color;
    Color origBossShieldColor;
    Animator anim;

    private GameObject EndScene;

    // Start is called before the first frame update
    void Awake()
    {
        health = 3.0f;
        EndScene = GameObject.FindGameObjectWithTag("EndScene");
        origBossShieldColor = bossShield.GetComponent<ParticleSystemRenderer>().material.GetColor("_TintColor");
        anim = this.GetComponent<Animator>();
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
    }

    public IEnumerator DamageShield()
    {
        yield return new WaitForSeconds(1.2f);
        //damageShield.SetActive(true);
        //bossShield.GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", color);
        LeanTween.value(bossShield, origBossShieldColor, color, .5f).setOnUpdate((Color val) => { bossShield.GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", val); });
        yield return new WaitForSeconds(.5f);
        LeanTween.value(bossShield, color, origBossShieldColor, .5f).setOnUpdate((Color val) => { bossShield.GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", val); });
        //damageShield.SetActive(false);
        //bossShield.GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", origBossShieldColor);
        //anim.SetBool("Hurt", false);
    }

    void UpdateMat(Color newColor)
    {
        
    }
}
