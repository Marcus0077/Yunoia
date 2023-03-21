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
    
    // Dynamic Clone Spawn Variables / position.
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
        // If the 'summon clone' button was pressed, attempt to summon a clone.
        if (summonAClone.WasPressedThisFrame())
        {
            SummonAClone();
        }
    }
    
    // Checks 4 points around the clone for a valid point to summon the clone.
    // If the point is valid (on the ground and not too close to a wall) then 
    // summon the clone.
    void SummonAClone()
    {
        // Check all 4 points around the clone.
        for (int i = 0; i < 4; i++)
        {
            // If the clone has not been summoned, check this spot with rays and summon it if
            // the spot is valid.
            if (!cloneSummoned)
            {
                Vector3 rayByPlayer = new Vector3(transform.position.x + cloneSpawnsX[i], transform.position.y,
                    transform.position.z + cloneSpawnsZ[i]);

                Vector3 direction = Vector3.zero;

                // Set the direction for the ray to cast in and clone to
                // be summoned in depending on what point around the clone 
                // we are currently checking.
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
                
                // Check if the current point is valid using raycasts.
                if ((Physics.Raycast(transform.position, direction, out hit, 1.5f, wall))
                    || (Physics.Raycast(transform.position, direction, out hit, 1.5f, ground))
                    || (!Physics.Raycast(rayByPlayer, Vector3.down, out hit, 1.5f, ground)
                        && !Physics.Raycast(rayByPlayer, Vector3.down, out hit, 1.5f, scale)))
                {
                    // Debug text if clone cannot be summoned.
                    Debug.Log("Clone cannot be summoned here.");
                }
                // If this the point is valid, summon the clone.
                else
                {
                    cloneSound.Play();

                    //Destroy the player's abilities.
                    this.GetComponent<Grapple>().DestroyHook();
                    this.GetComponent<AbilityPush>().DestroyShape();

                    // Disable 
                    basicMovementPlayer.canMove = false;
                    this.GetComponent<Grapple>().enabled = false;
                    this.GetComponent<AbilityPush>().enabled = false;

                    cloneSummoned = true;

                    // Summon clone.
                    Vector3 aboveGround = new Vector3(basicMovementPlayer.playerRB.position.x,
                        basicMovementPlayer.playerRB.position.y, basicMovementPlayer.playerRB.position.z) + direction;

                    RaycastHit spawnHit;

                    Physics.Raycast(new Vector3(aboveGround.x, aboveGround.y + 2f, aboveGround.z),
                        Vector3.down, out spawnHit, 2f, ground);

                    Vector3 newAboveGround = new Vector3(aboveGround.x, aboveGround.y + spawnHit.point.y + 0.5f, aboveGround.z);
                    
                    clone = Instantiate(ClonePrefab, newAboveGround, Quaternion.LookRotation(-Vector3.forward));

                    // Set clone UI if the animator exists.
                    if (uiAnim != null)
                    {
                        clone.GetComponent<CloneInteractions>().anim = uiAnim;
                        clone.GetComponent<ExitClone>().anim = uiAnim;
                        clone.GetComponent<CloneInteractions>().animCover = uiAnimCover;
                        clone.GetComponent<ExitClone>().animCover = uiAnimCover;
                        uiAnim.SetBool("isClone", true);
                        uiAnimCover.SetBool("isClone", true);
                    }

                    // Set camera to clone.
                    GameObject.FindGameObjectWithTag("Clone").GetComponent<BasicMovement>().CheckCameraState();
                    
                    StartCoroutine(Cooldown());
                }
            }
        }
    }

    // Subtracts from clone summon cooldown timer while it is above 0.
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
