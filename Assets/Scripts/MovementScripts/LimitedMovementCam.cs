using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LimitedMovementCam : MonoBehaviour
{
    // Input Variables
    PlayerControls playerControls;
    public InputAction camMove;
    public InputAction playerMove;
    
    // Camera Follow Sphere(s) RigidBody
    public Rigidbody camFollowSphere;

    // Movement Variables
    public Vector2 moveDirection;
    public float baseAccelerationValue;
    public float accelerationValueX;
    public float accelerationValueY;
    public float moveSpeed;
    public float returnSpeed;

    public Vector3 playerPos;

    public float returnToPlayerTimer;

    public CinemachineVirtualCameraBase curCamera;
    public CinemachineConfiner curConfiner;

    private float curConfinerScaleX;
    private float curConfinerPosX;
    private float curConfinerScaleY;
    private float curConfinerPosY;

    private float leftXBound;
    private float rightXBound;
    private float leftYBound;
    private float rightYBound;

    private float curCameraPosX;
    private float curCameraPosY;
    
    // Start is called before the first frame update
    void Awake()
    {
        playerControls = new PlayerControls();
        
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        transform.position = Vector3.MoveTowards(transform.position, playerPos, returnSpeed * Time.deltaTime);
        curCamera = GameObject.FindGameObjectWithTag("Camera1").GetComponent<CinemachineVirtualCamera>();

        GetCurrentConfinerData();

        returnSpeed = 20f;
        returnToPlayerTimer = 0f;
        baseAccelerationValue = 1f;
        accelerationValueX = 1f;
        accelerationValueY = 1f;
        moveSpeed = 5f;
    }

    // Currently Unused
    void GetCurrentCameraData()
    {
        
    }

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

    void ClampCameraFollowSphere()
    {
        Vector3 position = camFollowSphere.transform.position;

        position.x = Mathf.Clamp(position.x, leftXBound, rightXBound);
        position.y = Mathf.Clamp(position.y, leftYBound, rightYBound);
            
        camFollowSphere.transform.position = position;
    }

    void Update()
    {
        ClampCameraFollowSphere();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        float xInput = camMove.ReadValue<Vector2>().x;
        float yInput = camMove.ReadValue<Vector2>().y;

        curCameraPosX = curCamera.transform.position.x;
        curCameraPosY = curCamera.transform.position.y;
        
        
        if (camMove.IsPressed() && !playerMove.IsPressed())
        {
            returnToPlayerTimer = 1.5f;

            moveDirection = (Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y - 90, -Vector3.forward) *
                                 camMove.ReadValue<Vector2>().normalized * moveSpeed);

            camFollowSphere.velocity = new Vector3(moveDirection.y * accelerationValueY,
                     -moveDirection.x * accelerationValueX, camFollowSphere.velocity.z);

            if ((curConfinerScaleX - (curCameraPosX - curConfinerPosX) <= 0 && xInput == 1)
                || (curConfinerScaleX + (curCameraPosX - curConfinerPosX) <= 0 && xInput == -1))
            {
                Vector3 velocity = camFollowSphere.velocity;
                velocity.x = 0;
                camFollowSphere.velocity = velocity;
                
                accelerationValueY = 0f;
            }
            else if ((curConfinerScaleX -
                         (curCameraPosX - curConfinerPosX) <= 0 && xInput == -1) || (curConfinerScaleX + 
                         (curCameraPosX - curConfinerPosX) <= 0 && xInput == 1) || (curConfinerScaleX -
                         (curCameraPosX - curConfinerPosX) >= 0 || (curCameraPosX - curConfinerPosX) >= 0))
            {
                Vector3 velocity = camFollowSphere.velocity;
                velocity.x = moveDirection.y * accelerationValueY;
                camFollowSphere.velocity = velocity;
                
                accelerationValueY = 1f;
            }
            
            if ((curConfinerScaleY - (curCameraPosY - curConfinerPosY) <= 0 && yInput == 1)
                || (curConfinerScaleY + (curCameraPosY - curConfinerPosY) <= 0 && yInput == -1))
            {
                accelerationValueX = 0f;
            }
            else if ((curConfinerScaleY -
                         (curCameraPosY - curConfinerPosY) <= 0 && yInput == -1) || (curConfinerScaleY + 
                         (curCameraPosY - curConfinerPosY) <= 0 && yInput == 1) || (curConfinerScaleY -
                         (curCameraPosY - curConfinerPosY) >= 0 || (curCameraPosY - curConfinerPosY) >= 0))
            {
                accelerationValueX = 1f;
            }
        }
        else if (!camMove.IsPressed() && !playerMove.IsPressed() && returnToPlayerTimer > 0)
        {
            camFollowSphere.velocity = Vector2.zero;
            
            returnToPlayerTimer -= Time.deltaTime;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, playerPos, returnSpeed * Time.deltaTime);
        }
    }
    
    // Enable input action map controls.
    private void OnEnable()
    {
        playerMove = playerControls.Movement.Move;
        playerMove.Enable();
        
        camMove = playerControls.Movement.CamMove;
        camMove.Enable();
    }

    // Disable input action map controls.
    private void OnDisable()
    {
        playerMove.Disable();
        camMove.Disable();
    }
}
