using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;

public class SummonClone : MonoBehaviour
{
    // Script references
    private BasicMovement basicMovementPlayer;

    // Input variables
    PlayerControls summonControls;
    private InputAction summonAClone;

    // Clone Summon variables
    public GameObject ClonePrefab;
    public GameObject clone;
    public bool cloneSummoned;
    public float cooldown = 0;
    float cdRemaining;

    // LayerMask variables
    public LayerMask ground;
    public LayerMask wall;

    [SerializeField] AudioSource cloneSound;

    // Get references and initialize variables when player spawns.
    void Awake()
    {
        basicMovementPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<BasicMovement>();

        summonControls = new PlayerControls();
        
        cloneSummoned = false;
    }

    // Called each frame.
    void Update()
    {
        if (!cloneSummoned && summonAClone.WasPressedThisFrame())
        {
            SummonAClone();
        }
        
    }
    
    // Summon a clone at a specified location if they are not too close to a wall 
    // and if the clone will spawn on the ground.
    // Freezes player and deactivates ability to summon a clone.
    void SummonAClone()
    {
        Vector3 rightOfPlayer = new Vector3(transform.position.x, transform.position.y - 0.25f, 
            transform.position.z - 1f);
        
        RaycastHit hit;

        Debug.DrawRay(transform.position, Vector3.back * 1.25f, Color.green, 2f);
        Debug.DrawRay(rightOfPlayer, Vector3.down * 1f, Color.green, 2f);
        
        
        if (Physics.Raycast(transform.position, Vector3.back, out hit, 1.25f) || 
            !Physics.Raycast(rightOfPlayer, Vector3.down, out hit, 1.25f, ground))
        {
            Debug.Log("Did Hit");
        }
        else
        {
            cloneSound.Play();
            
            this.GetComponent<Grapple>().DestroyHook();
            this.GetComponent<AbilityPush>().DestroyShape();

            basicMovementPlayer.canMove = false;
            this.GetComponent<Grapple>().enabled = false;
            this.GetComponent<AbilityPush>().enabled = false;
        
            cloneSummoned = true;
        
            clone = Instantiate(ClonePrefab, basicMovementPlayer.playerRB.position + Vector3.back, 
                Quaternion.identity);
            StartCoroutine(Cooldown());
        }
    }

    private IEnumerator Cooldown()
    {
        cdRemaining = cooldown;
        while (cdRemaining > 0)
        {
            cdRemaining -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public float CooldownRemaining()
    {
        if (cdRemaining > 0)
        {
            return cdRemaining;
        }
        else
        {
            return -1;
        }
    }

    // Enable input action map controls.
    private void OnEnable()
    {
        summonAClone = summonControls.SummonClone.SummonAClone;
        summonAClone.Enable();
    }

    // Disable input action map controls.
    private void OnDisable()
    {
        summonAClone.Disable();
    }
}
