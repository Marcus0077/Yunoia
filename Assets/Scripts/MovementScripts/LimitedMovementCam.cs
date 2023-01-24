using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LimitedMovementCam : MonoBehaviour
{
    // Input Variables.
    PlayerControls playerControls;
    public InputAction camMove;
    public InputAction playerMove;
    public InputAction cloneButton;
    
    // Camera Follow Sphere RigidBody.
    public Rigidbody camFollowSphere;

    // Sphere Movement Variables.
    public Vector2 moveDirection;
    public float accelerationValue;
    public float moveSpeed;
    public float returnSpeed;

    // Player Information.
    public GameObject Player;
    public Vector3 playerPos;

    // Return Sphere to player Cooldown Timer.
    public float returnToPlayerTimer;

    // Camera Variables.
    public CinemachineVirtualCameraBase curCamera;

    // Confiner Variables.
    public CinemachineConfiner curConfiner;
    private float curConfinerScaleX;
    private float curConfinerPosX;
    private float curConfinerScaleY;
    private float curConfinerPosY;

    // Bounding Box Variables.
    private float leftXBound;
    private float rightXBound;
    private float leftYBound;
    private float rightYBound;

    // Offset Values.
    private Vector3 camBallOffset;

    
    public bool isCamFollowingPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        playerControls = new PlayerControls();

        Player = GameObject.FindWithTag("Player");
        playerPos = Player.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, playerPos, returnSpeed * Time.deltaTime);
        curCamera = GameObject.FindGameObjectWithTag("Camera1").GetComponent<CinemachineVirtualCamera>();
        curCamera.Follow = Player.transform;
        
        camBallOffset = curCamera.transform.position - camFollowSphere.transform.position;

        GetCurrentConfinerData();

        returnSpeed = 20f;
        returnToPlayerTimer = 0f;
        accelerationValue = 1f;
        moveSpeed = 10f;

        isCamFollowingPlayer = true;
    }

    // Currently unused; will need later for switching cameras.
    void GetCurrentCameraData()
    {
        
    }

    // Grab all needed information about the current camera's confiner.
    void GetCurrentConfinerData()
    {
        curConfiner = curCamera.GetComponent<CinemachineConfiner>();
        
        curConfinerScaleX = curConfiner.m_BoundingVolume.transform.lossyScale.x * .5f;
        curConfinerPosX = (curConfiner.m_BoundingVolume.transform.position.x);
        
        curConfinerScaleY = curConfiner.m_BoundingVolume.transform.lossyScale.y * .5f;
        curConfinerPosY = (curConfiner.m_BoundingVolume.transform.position.y);

        leftXBound = (curConfinerPosX - curConfinerScaleX);
        rightXBound = (curConfinerPosX + curConfinerScaleX);
        
        leftYBound = (curConfinerPosY - curConfinerScaleY);
        rightYBound = (curConfinerPosY + curConfinerScaleY);
    }

    // Clamp the position of the camera follow sphere to the bounds of the confiner.
    void ClampCameraFollowSphere()
    {
        Vector3 position = camFollowSphere.transform.position;

        position.x = Mathf.Clamp(position.x, leftXBound, rightXBound);
        position.y = Mathf.Clamp(position.y, leftYBound - camBallOffset.y, rightYBound - camBallOffset.y);
            
        camFollowSphere.transform.position = position;
    }

    // Update is called each frame.
    void Update()
    {
        if (isCamFollowingPlayer == false)
        {
            ClampCameraFollowSphere();
        }
    }

    // FixedUpdate is called between frames.
    void FixedUpdate()
    {
        playerPos = Player.transform.position;
        
        // If the camera follow sphere is being controlled and the player is not, set the camera target to the
        // camera follow sphere and move the sphere along a 2D plane (x, y) according to player input via arrows
        // or d-pad. Return timer remains at a static value as long as the camera is being moved.
        if (camMove.IsPressed() && !playerMove.IsPressed())
        {
            if (isCamFollowingPlayer)
            {
                curCamera.Follow = transform;

                isCamFollowingPlayer = false;
            }
            
            returnToPlayerTimer = 1.5f;

            moveDirection = (Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y - 90, -Vector3.forward) *
                                 camMove.ReadValue<Vector2>().normalized * moveSpeed);

            camFollowSphere.velocity = new Vector3(moveDirection.y * accelerationValue,
                     -moveDirection.x * accelerationValue, camFollowSphere.velocity.z);
        }
        // If there is no movement input for the camera follow sphere or the character,
        // stop the ball and countdown the return timer.
        else if (!camMove.IsPressed() && !playerMove.IsPressed() && returnToPlayerTimer > 0)
        {
            camFollowSphere.velocity = Vector2.zero;
            
            returnToPlayerTimer -= Time.deltaTime;
            
        }
        // If the return timer runs out or the player is moving the character, the camera target will become the
        // character again and camera follow sphere will follow the player.
        else
        {
            if (!isCamFollowingPlayer)
            {
                curCamera.Follow = Player.transform;

                isCamFollowingPlayer = true;
            }
            
            transform.position = Vector3.MoveTowards(transform.position, playerPos, 
                returnSpeed * Time.deltaTime);
        }
    }
    
    // Enable input action map controls.
    private void OnEnable()
    {
        playerMove = playerControls.Movement.Move;
        playerMove.Enable();
        
        camMove = playerControls.Movement.CamMove;
        camMove.Enable();

        cloneButton = playerControls.SummonClone.SwitchPlaces;
        cloneButton.Enable();
    }

    // Disable input action map controls.
    private void OnDisable()
    {
        playerMove.Disable();
        camMove.Disable();
        cloneButton.Disable();
    }
}
