using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class HideWall : MonoBehaviour
{
    // Input variables
    PlayerControls playerControls;
    private InputAction exit;

    [SerializeField] private Transform Obstruction;

    private void Start()
    {
        playerControls = new PlayerControls();
        
        exit = playerControls.Camera.Exit;
        exit.Enable();
    }

    private void FixedUpdate()
    {
        if (CinemachineCore.Instance.IsLive(this.GetComponent<CinemachineVirtualCamera>()))
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position,
                    this.GetComponent<CinemachineVirtualCamera>().Follow.transform.position - transform.position,
                    out hit, 8f))
            {
                Debug.DrawRay(transform.position,
                    this.GetComponent<CinemachineVirtualCamera>().Follow.transform.position - transform.position,
                    Color.green);

                if (hit.collider.gameObject.tag != "Player" && hit.collider.gameObject.tag != "Clone" &&
                    !hit.collider.isTrigger)
                {
                    Obstruction = hit.transform;
                    Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode =
                        UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }
            }
            else if (Obstruction != null)
            {
                Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode =
                    UnityEngine.Rendering.ShadowCastingMode.On;

            }
        }
    }

    // Disable input action map controls.
    public void OnDisable()
    {
        exit.Disable();
    }
}
