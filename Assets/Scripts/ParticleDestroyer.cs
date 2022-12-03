using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    public GameObject cloneChargeParticlesToKill;
    public GameObject cloneSmallPushParticlesToKill;
    public GameObject cloneLargePushParticlesToKill;
    public GameObject cloneGrappleParticlesToKill;
    
    public GameObject playerChargeParticlesToKill;
    public GameObject PlayerSmallPushParticlesToKill;
    public GameObject playerLargePushParticlesToKill;
    public GameObject playerGrappleParticlesToKill;

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindWithTag("Clone") != null)
        {
            if (GameObject.FindWithTag("Clone").GetComponent<AbilityPush>().chargePushNeedsDeath)
            {
                GameObject.FindWithTag("Clone").GetComponent<AbilityPush>().chargePushNeedsDeath = false;
                cloneChargeParticlesToKill = GameObject.FindWithTag("Clone").GetComponent<AbilityPush>().chargeEffectDestroy;
                StartCoroutine(ExecuteParticlesInstant(cloneChargeParticlesToKill));
            }
            if (GameObject.FindWithTag("Clone").GetComponent<AbilityPush>().smallPushNeedsDeath)
            {
                GameObject.FindWithTag("Clone").GetComponent<AbilityPush>().smallPushNeedsDeath = false;
                cloneSmallPushParticlesToKill = GameObject.FindWithTag("Clone").GetComponent<AbilityPush>().smallEffectDestroy;
                StartCoroutine(ExecuteParticlesDelayed(cloneSmallPushParticlesToKill));
            }
            if (GameObject.FindWithTag("Clone").GetComponent<AbilityPush>().largePushNeedsDeath)
            {
                GameObject.FindWithTag("Clone").GetComponent<AbilityPush>().largePushNeedsDeath = false;
                cloneLargePushParticlesToKill = GameObject.FindWithTag("Clone").GetComponent<AbilityPush>().largeEffectDestroy;
                StartCoroutine(ExecuteParticlesDelayed(cloneLargePushParticlesToKill));
            }
            if (GameObject.FindWithTag("Clone").GetComponent<Grapple>().grappleNeedsDeath)
            {
                GameObject.FindWithTag("Clone").GetComponent<Grapple>().grappleNeedsDeath = false;
                cloneGrappleParticlesToKill = GameObject.FindWithTag("Clone").GetComponent<Grapple>().grappleEffectDestroy;
                StartCoroutine(ExecuteParticlesDelayed(cloneGrappleParticlesToKill));
            }
        }
        
        if (GameObject.FindWithTag("Player").GetComponent<AbilityPush>().chargePushNeedsDeath)
        {
            GameObject.FindWithTag("Player").GetComponent<AbilityPush>().chargePushNeedsDeath = false;
            playerChargeParticlesToKill = GameObject.FindWithTag("Player").GetComponent<AbilityPush>().chargeEffectDestroy;
            StartCoroutine(ExecuteParticlesInstant(playerChargeParticlesToKill));
        }
        if (GameObject.FindWithTag("Player").GetComponent<AbilityPush>().smallPushNeedsDeath)
        {
            GameObject.FindWithTag("Player").GetComponent<AbilityPush>().smallPushNeedsDeath = false;
            PlayerSmallPushParticlesToKill = GameObject.FindWithTag("Player").GetComponent<AbilityPush>().smallEffectDestroy;
            StartCoroutine(ExecuteParticlesDelayed(PlayerSmallPushParticlesToKill));
        }
        if (GameObject.FindWithTag("Player").GetComponent<AbilityPush>().largePushNeedsDeath)
        {
            GameObject.FindWithTag("Player").GetComponent<AbilityPush>().largePushNeedsDeath = false;
            playerLargePushParticlesToKill = GameObject.FindWithTag("Player").GetComponent<AbilityPush>().largeEffectDestroy;
            StartCoroutine(ExecuteParticlesDelayed(playerLargePushParticlesToKill));
        }
        if (GameObject.FindWithTag("Player").GetComponent<Grapple>().grappleNeedsDeath)
        {
            GameObject.FindWithTag("Player").GetComponent<Grapple>().grappleNeedsDeath = false;
            playerGrappleParticlesToKill = GameObject.FindWithTag("Player").GetComponent<Grapple>().grappleEffectDestroy;
            StartCoroutine(ExecuteParticlesDelayed(playerGrappleParticlesToKill));
        }
    }

    IEnumerator ExecuteParticlesDelayed(GameObject particlesToExecute)
    {
        yield return new WaitForSeconds(2);
        if(particlesToExecute != null && particlesToExecute.GetComponent<ParticleSystem>() != null)
        {
            particlesToExecute.GetComponent<ParticleSystem>().Stop();
        }
        yield return new WaitForSeconds(.1f);
        if (particlesToExecute != null)
        {
            Destroy(particlesToExecute);
        }
        else
        {
            yield break;
        }
    }

    IEnumerator ExecuteParticlesInstant(GameObject particlesToExecute)
    {
        yield return new WaitForSeconds(0);
        Destroy(particlesToExecute);
    }
}
