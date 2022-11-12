using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractDoor : MonoBehaviour
{
    PlayerControls controls;
    InputAction press;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        controls = new PlayerControls();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (press.IsPressed())
            {

            }
        }
    }

    private void OnEnable()
    {
        press = controls.Interaction.Press;
        press.Enable();
    }

    private void OnDisable()
    {
        press.Disable();
    }
}
