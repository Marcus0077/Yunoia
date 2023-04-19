using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathParticle : MonoBehaviour
{
    ParticleSystem ps;
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ps.trigger.AddCollider(GameManager.Instance.Player.GetComponent<Collider>());
    }
    // Actions to take place upon touching the collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //StartCoroutine(FadeThenDie());
            GameObject.FindObjectOfType<PauseMenu>().DisableInput();
            FadetoBlack();
        }
        else if (other.CompareTag("Clone"))
        {
            GameObject.FindObjectOfType<ExitClone>().despawnClone = true;
        }
        else if (other.CompareTag("BoxScale"))
        {
            BoxRespawner box = other.gameObject.GetComponent<BoxRespawner>();
            if (box != null)
            {
                 box.Reset();
            }
        }
    }

    public void FadetoBlack()
    {
        //StartCoroutine(FadeThenDie());
        GameManager.Instance.StartDeath();
    }

    void OnParticleTrigger()
    {
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        GameObject player = GameManager.Instance.Player;
        if (numEnter > 0)
        {
            if (player.GetComponent<Collider>().bounds.Contains(new Vector3(player.transform.position.x, enter[0].position.y, player.transform.position.z)))
            {
                FadetoBlack();
                return;
            }
            Debug.Log("Not real hit");
        }
    }

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
