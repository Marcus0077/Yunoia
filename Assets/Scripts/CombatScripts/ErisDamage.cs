using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErisDamage : MonoBehaviour
{
    private ErisBoss boss;
    private ErisAttackController bossAttack;
    public int puzzleNum;

    private bool used;

    private void Start()
    {
        used = false;
        boss = GameObject.FindObjectOfType<ErisBoss>();
        bossAttack = GameObject.FindObjectOfType<ErisAttackController>();
    }

    public void DoDamage()
    {
        if (!used)
            StartCoroutine(CompletePuzzle());
    }

    //private void OnTriggerEnter(Collider collider)
    //{
    //    if (collider.CompareTag("Player") && !used)
    //    {
    //        StartCoroutine(CompletePuzzle());
    //    }
    //}

    
    private IEnumerator CompletePuzzle()
    {
        if(!used)
        {
            used = true;
        }
        else
        {
            yield break;
        }
        float waitTime = 2.5f;
        bossAttack.attackFrozen = true;
        
        GameManager.Instance.ShowPuzzleWrapper(puzzleNum, waitTime);

        yield return new WaitForSeconds(waitTime);

        boss.ErisHurt();
        
        Destroy(gameObject);
        //this.gameObject.GetComponentsInChildren<Renderer>()[0].material.SetColor("_EmissionColor", Color.black);
        //this.gameObject.GetComponentsInChildren<Renderer>()[1].material.SetColor("_EmissionColor", Color.black);

        yield return new WaitForSeconds(waitTime);

        bossAttack.attackFrozen= false;
    }
}
