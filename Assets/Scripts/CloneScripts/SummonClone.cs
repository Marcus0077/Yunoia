using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class SummonClone : MonoBehaviour
{
    // Script references
    private BasicMovement basicMovementPlayer;

    // Input variables
    public PlayerControls summonControls;
    public InputAction summonAClone;

    // Clone Summon variables
    public GameObject ClonePrefab;
    public GameObject clone;
    public bool cloneSummoned;
    
    // Cooldown variables.
    public float cooldown = 0;
    float cdRemaining;

    // LayerMask variables
    public LayerMask ground;
    public LayerMask wall;
    public LayerMask scale;

    // Audio variables.
    [SerializeField] AudioSource cloneSound;
    [SerializeField] Animator uiAnim;
    [SerializeField] Animator uiAnimCover;
    
    // Dynamic Clone Spawn Variables
    private float[] cloneSpawnsX = { 1.5f, -1.5f, 0, 0};
    private float[] cloneSpawnsZ = { 0, 0, 1.5f, -1.5f};

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
        if (summonAClone.WasPressedThisFrame())
        {
            SummonAClone();
        }
    }
    
    // Summons a clone at a specified location if they are not too close to a solid object 
    // and if the clone will spawn on the ground, using raycasts.
    // Freezes player and deactivates ability to summon a clone.
    void SummonAClone()
    {
        for (int i = 0; i < 4; i++)
        {
            if (!cloneSummoned)
            {
                Vector3 rayByPlayer = new Vector3(transform.position.x + cloneSpawnsX[i], transform.position.y,
                    transform.position.z + cloneSpawnsZ[i]);

                Vector3 direction = Vector3.zero;

                if (i == 0)
                {
                    direction = Vector3.right;
                }
                else if (i == 1)
                {
                    direction = Vector3.left;
                }
                else if (i == 2)
                {
                    direction = Vector3.forward;
                }
                else if (i == 3)
                {
                    direction = Vector3.back;
                }

                RaycastHit hit;


                // Debug rays.
                Debug.DrawRay(transform.position, direction * 1.5f, Color.green, 1.5f);
                Debug.DrawRay(rayByPlayer, Vector3.down * 1.5f, Color.green, 1.5f);


                if ((Physics.Raycast(transform.position, direction, out hit, 1.5f, wall))
                    || (Physics.Raycast(transform.position, direction, out hit, 1.5f, ground))
                    || (!Physics.Raycast(rayByPlayer, Vector3.down, out hit, 1.5f, ground)
                        && !Physics.Raycast(rayByPlayer, Vector3.down, out hit, 1.5f, scale)))
                {
                    // Debug text if clone cannot be summoned.
                    Debug.Log("Clone cannot be summoned here.");
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

                    clone = Instantiate(ClonePrefab, basicMovementPlayer.playerRB.position + direction,
                        Quaternion.LookRotation(-Vector3.forward));

                    if (uiAnim != null)
                    {
                        clone.GetComponent<CloneInteractions>().anim = uiAnim;
                        clone.GetComponent<ExitClone>().anim = uiAnim;
                        clone.GetComponent<CloneInteractions>().animCover = uiAnimCover;
                        clone.GetComponent<ExitClone>().animCover = uiAnimCover;
                        uiAnim.SetBool("isClone", true);
                        uiAnimCover.SetBool("isClone", true);
                    }

                    GameObject.FindGameObjectWithTag("Clone").GetComponent<BasicMovement>().CheckCameraState();

                    StartCoroutine(Cooldown());
                }
            }
        }
    }

    // Subtracts from clone summon cooldown timer while it is above 0, 
    // and returns the current timer value.
    private IEnumerator Cooldown()
    {
        cdRemaining = cooldown;
        while (cdRemaining > 0)
        {
            cdRemaining -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    // Returns the remaining cooldown time as long as it is above 0.
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
