using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Door : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    TextMeshProUGUI activateText;
    public bool canInteract;
    private bool isPlayer;
    private bool isClone;
    private CloneInteractions cloneInteractions;
    private PlayerInteractions playerInteractions;
    // Start is called before the first frame update
    void Start()
    {
        if(activateText != null)
            activateText.enabled = false;
        isPlayer = false;
        isClone = false;
    }

    public void Open()
    {
        animator.SetTrigger("DoorOpen");
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (activateText != null && canInteract)
                activateText.enabled = true;

            playerInteractions = other.GetComponent<PlayerInteractions>();
            playerInteractions.canPressDoor = true;

            isPlayer = true;
        }
        else if (other.CompareTag("Clone"))
        {
            if(activateText != null && canInteract)
                activateText.enabled = true;

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