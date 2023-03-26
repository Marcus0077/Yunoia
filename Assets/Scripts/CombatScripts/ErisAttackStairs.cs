using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErisAttackStairs : MonoBehaviour
{
    public Rigidbody playerRB;
    public Transform playerTrans;
    public Transform spawnPoint;

    public GameObject attackPrefab;
    public GameObject attackSpawned;

    public bool canAttack = true;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitalPause());
    }

    void FixedUpdate()
    {
        if (canAttack)
        {
            attackSpawned = Instantiate(attackPrefab, 
            new Vector3(playerTrans.position.x, playerTrans.position.y + 10.0f, playerTrans.position.z), 
            Quaternion.identity);
            
            attackSpawned.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, -3.0f * Time.fixedDeltaTime, 0.0f);

            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator InitalPause()
    {
        yield return new WaitForSeconds(5.0f);

        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;

        yield return new WaitForSeconds(2.5f);

        canAttack = true;
    }
}
