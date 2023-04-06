using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class HideObstructions : MonoBehaviour
{
    // Array of Transforms that are obstructing the view of the camera.
    public Transform[] Obstructions;

    // Decides whether to render at limited opacity or fully dissappear object(s).
    public bool partiallyDissappear;

    // Decides whether this trigger makes objects appear or disappear.
    public bool reAppear;

    // The other trigger for this trigger's room.
    public GameObject counterpartObject;
    private HideObstructions Counterpart;
    
    // Tells whether player or clone is in the anger room.
    public bool isPlayer;
    public bool isClone;
    public bool allowCloneIn;

    private Animator cameraAnimator;
    public int thisRoom;

    // Called when Hide Obstruction triggers are initialised.
    private void Start()
    {
        isPlayer = false;
        isClone = false;

        if (counterpartObject != null)
        {
            if (partiallyDissappear)
            {
                Counterpart = counterpartObject.GetComponents<HideObstructions>()[0];
            }
            else
            {
                Counterpart = counterpartObject.GetComponents<HideObstructions>()[1];
            }
        }
        
        if (GameObject.FindObjectOfType<CinemachineStateDrivenCamera>().GetComponent<Animator>() != null)
        {
            cameraAnimator = GameObject.FindObjectOfType<CinemachineStateDrivenCamera>().GetComponent<Animator>();
        }
        
        if (cameraAnimator.GetInteger("roomNum") == thisRoom && thisRoom != 0)
        {
            FullyHideWalls();
        }
    }

    // If the player or clone enters an area where there are things obstructing the camera,
    // set these objects to be invisible and only cast shadows. If they have a collider, 
    // turn the collider off.
    private void OnTriggerEnter(Collider other)
    {
        if (!partiallyDissappear && !reAppear)
        {
            if (other.CompareTag("Player"))
            {
                isPlayer = true;
                
                other.GetComponent<BasicMovement>().inAngerRoom = true;
                other.GetComponent<BasicMovement>().curAngerRoomFullTrigger = this;
                
                FullyHideWalls();
            }
            else if (other.CompareTag("Clone") && allowCloneIn)
            {
                isClone = true;
                
                other.GetComponent<BasicMovement>().inAngerRoom = true;
                other.GetComponent<BasicMovement>().curAngerRoomFullTrigger = this;
                
                FullyHideWalls();
            }
        }
        if (partiallyDissappear && !reAppear)
        {
            if (other.CompareTag("Player"))
            {
                isPlayer = true;
                
                other.GetComponent<BasicMovement>().inAngerRoom = true;
                other.GetComponent<BasicMovement>().curAngerRoomPartialTrigger = this;
                
                PartialHideWalls();
            }
            else if (other.CompareTag("Clone") && allowCloneIn)
            {
                isClone = true;
                
                other.GetComponent<BasicMovement>().inAngerRoom = true;
                other.GetComponent<BasicMovement>().curAngerRoomPartialTrigger = this;
                
                PartialHideWalls();
            }
        }
        if (!partiallyDissappear && reAppear)
        {
            if (other.CompareTag("Player"))
            {
                Counterpart.isPlayer = false;
                
                other.GetComponent<BasicMovement>().inAngerRoom = false;
                other.GetComponent<BasicMovement>().curAngerRoomFullTrigger = null;

                if (!Counterpart.isClone)
                {
                    FullyViewWalls();
                }
            }
            else if (other.CompareTag("Clone") && allowCloneIn)
            {
                Counterpart.isClone = false;
                
                other.GetComponent<BasicMovement>().inAngerRoom = false;
                other.GetComponent<BasicMovement>().curAngerRoomFullTrigger = null;
                
                if (!Counterpart.isPlayer)
                {
                    FullyViewWalls();
                }
            }
        }
        if (partiallyDissappear && reAppear)
        {
            if (other.CompareTag("Player"))
            {
                Counterpart.isPlayer = false;
                
                other.GetComponent<BasicMovement>().inAngerRoom = false;
                other.GetComponent<BasicMovement>().curAngerRoomPartialTrigger = null;
                
                if (!Counterpart.isClone)
                {
                    PartialViewWalls();   
                }
            }
            else if (other.CompareTag("Clone") && allowCloneIn)
            {
                Counterpart.isClone = false;
                
                other.GetComponent<BasicMovement>().inAngerRoom = false;
                other.GetComponent<BasicMovement>().curAngerRoomPartialTrigger = null;
                
                if (!Counterpart.isPlayer)
                {
                    PartialViewWalls();
                }
            }
        }
    }

    public void PartialHideWalls()
    {
        foreach (var obstruction in Obstructions)
        {
            Color obstructionColor = obstruction.GetComponent<MeshRenderer>().material.color;
            obstructionColor.a = 0.25f;

            obstruction.GetComponent<MeshRenderer>().material.color = obstructionColor;
        }
    }
    
    public void FullyHideWalls()
    {
        foreach (var obstruction in Obstructions)
        {
            obstruction.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly;

            if (obstruction.GetComponent<MeshCollider>())
            {
                obstruction.GetComponent<MeshCollider>().enabled = false;
            }
        }
    }
    
    public void PartialViewWalls()
    {
        foreach (var obstruction in Obstructions)
        {
            Color obstructionColor = obstruction.GetComponent<MeshRenderer>().material.color;
            obstructionColor.a = 1f;

            obstruction.GetComponent<MeshRenderer>().material.color = obstructionColor;
        }
    }
    
    public void FullyViewWalls()
    {
        foreach (var obstruction in Obstructions)
        {
            obstruction.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.On;

            if (obstruction.GetComponent<MeshCollider>())
            {
                obstruction.GetComponent<MeshCollider>().enabled = true;
            }
        }
    }
}
