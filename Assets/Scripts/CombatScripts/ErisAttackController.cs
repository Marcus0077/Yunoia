using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErisAttackController : MonoBehaviour
{
    public Rigidbody playerRB;
    public Transform playerTrans;

    public ErisAttackStairs attackPrefab;

    public bool canAttack = false;
    float velocity = -8.0f;

    public bool hit = false;

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
    }

    private IEnumerator InitalPause()
    {
        yield return new WaitForSeconds(5.0f);

        canAttack = true;
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;

        yield return new WaitForSeconds(2.0f);

        canAttack = true;
    }
}
