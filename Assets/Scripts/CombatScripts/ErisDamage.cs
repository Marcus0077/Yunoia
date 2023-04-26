using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErisDamage : MonoBehaviour
{
    private ErisBoss boss;
    private ErisAttackController bossAttack;
    public int puzzleNum;
    Animator anim;

    private bool used;

    private void Start()
    {
        used = false;
        boss = GameObject.FindObjectOfType<ErisBoss>();
        bossAttack = GameObject.FindObjectOfType<ErisAttackController>();
        anim = boss.GetComponent<Animator>();
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
        
        Destroy(gameObject.GetComponentInChildren<MeshRenderer>());

        float waitTime = 2.5f;

        bossAttack.attackFrozen = true;
        
        GameManager.Instance.ShowPuzzleWrapper(puzzleNum, waitTime);
        
        anim.SetTrigger("Hurt");
        boss.StartCoroutine(boss.DamageShield());
        yield return new WaitForSeconds(waitTime);

        boss.ErisHurt();

        yield return new WaitForSeconds(waitTime);

        bossAttack.attackFrozen = false;

        Destroy(gameObject);
    }
}
