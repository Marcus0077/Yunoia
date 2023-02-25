using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Anything triggered by an interactable
public class Door : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    public Image activateText;
    public bool canInteract;
    public bool isPlayer;
    public bool isClone;
    private CloneInteractions cloneInteractions;
    private PlayerInteractions playerInteractions;
    public AudioClip doorSound;
    AudioSource audioSource;

    private bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        
        audioSource = GetComponent<AudioSource>();

        if (activateText != null)
        {
            activateText.enabled = false;
        }

        isPlayer = false;
        isClone = false;
    }

    // Plays animation caused by trigger DoorOpen
    public void Open()
    {
        if (!isOpen)
        {
            isOpen = true;

            if (activateText != null)
            {
                Destroy(activateText);
            }

            animator.SetTrigger("DoorOpen");

            audioSource.PlayOneShot(doorSound, 0.4f);
        }
    }

    public void Close()
    {
        animator.SetTrigger("DoorClose");
    }

    // Update is called once per frame
    void Update()
    {
        if (cloneInteractions == null && isClone)
        {
            isClone = false;
        }

    }

    // Allow a text to display if needed
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (activateText != null && canInteract)
            {
                activateText.enabled = true;
            }

            playerInteractions = other.GetComponent<PlayerInteractions>();
            playerInteractions.canPressDoor = true;
            
            isPlayer = true;
        }
        else if (other.CompareTag("Clone"))
        {
            if (activateText != null && canInteract)
            {
                activateText.enabled = true;
            }

            cloneInteractions = other.GetComponent<CloneInteractions>();
            cloneInteractions.canPressDoor = true;

            isClone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayer = false;
            playerInteractions.canPressDoor = false;
        }
        else if (other.CompareTag("Clone"))
        {
            isClone = false;
            cloneInteractions.canPressDoor = false;
            
        }
        
        if (!isPlayer && !isClone && canInteract && activateText != null)
        {
            activateText.enabled = false;
        }
    }
}
