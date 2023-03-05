using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Ghost mode for debugging
public class GhostMode : MonoBehaviour
{
    public PlayerControls playerControls;
    InputAction move, up, down, camera, shift, mouseSense, exit;
    public float speed, vSpeed, sensitivity; // read from gameManager?
    Vector3 moveAmount;
    Vector2 totalRot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerControls = new PlayerControls();
        move = playerControls.Ghost.Move;
        move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        move.canceled += ctx => Move(Vector2.zero);
        shift = playerControls.Ghost.Sprint;
        shift.performed += ctx => speed *= 2;
        shift.canceled += ctx => speed /= 2;
        mouseSense = playerControls.Ghost.Sensitivity;
        mouseSense.performed += ctx => AddMouseSensitivity(ctx.ReadValue<float>());
        up = playerControls.Ghost.Up;
        down = playerControls.Ghost.Down;
        up.performed += ctx => Elevate(vSpeed);
        up.canceled += ctx => Elevate(0);
        down.performed += ctx => Elevate(-vSpeed);
        down.canceled += ctx => Elevate(0);
        camera = playerControls.Ghost.Camera;
        camera.performed += ctx => { totalRot += ctx.ReadValue<Vector2>() * sensitivity; transform.rotation = Quaternion.Euler(-(totalRot.y - 360 * Mathf.Floor(totalRot.y / 360)), totalRot.x - 360 * Mathf.Floor(totalRot.x / 360), 0); };
        exit = playerControls.Ghost.Exit;
        exit.performed += ctx => ToggleGhost();
    }

    void ToggleGhost()
    {
        GameManager.Instance.ToggleGhost();
    }

    void AddMouseSensitivity(float value)
    {
        sensitivity = Mathf.Clamp(sensitivity + value * .1f,0,1);
    }

    void Elevate(float amount)
    {
        moveAmount.y = amount * vSpeed;
    }

    void Move(Vector2 amount)
    {
        moveAmount.x = amount.x;
        moveAmount.z = amount.y;
        //moveAmount = Vector3.RotateTowards(moveAmount, transform.forward, 1, 1);
    }

    // Enable input action map controls.
    private void OnEnable()
    {
        move.Enable();
        up.Enable();
        down.Enable();
        camera.Enable();
        shift.Enable();
        mouseSense.Enable();
        exit.Enable();
    }

    // Disable input action map controls.
    private void OnDisable()
    {
        move.Disable();
        up.Disable();
        down.Disable();
        camera.Disable();
        shift.Disable();
        mouseSense.Disable();
        exit.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (moveAmount.x * transform.right).normalized * speed * Time.unscaledDeltaTime;
        transform.position += (moveAmount.z * transform.forward).normalized * speed * Time.unscaledDeltaTime;
        transform.position += moveAmount.y * Vector3.up * Time.unscaledDeltaTime;
    }
}
