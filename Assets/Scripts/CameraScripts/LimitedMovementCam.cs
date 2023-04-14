using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LimitedMovementCam : MonoBehaviour
{
    // Input Variables.
    public PlayerControls playerControls;
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

    // Reference to the current camera.
    public CinemachineVirtualCameraBase curCamera;

    // Camera Confiner Variables.
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

    // Offset value between the sphere and the camera.
    private Vector3 camBallOffset;

    // Checks if camera is following player or sphere.
    public bool isCamFollowingPlayer;

    // Make ball travel back to player if clone actions are being used
    // or player is moving.
    public bool forceBallToPlayer;

    public bool canUseFreeCam;

    // Get references and initialize variables when the Camera is initialised.

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    void Start()
    {
        EnablePlayerControls();

        Player = GameObject.FindWithTag("Player");
        playerPos = Player.transform.position;
        
        transform.position = Vector3.MoveTowards
            (transform.position, playerPos, returnSpeed * Time.deltaTime);

        // If we are starting from a checkpoint, set the camera to
        // that checkpoint's camera.
        if (DataManager.gameData.checkpointed && DataManager.gameData.checkpointDatas[GameManager.Instance.currentLevel].room >= 0)
        {
            GetCurrentCameraData(DataManager.gameData.checkpointDatas[GameManager.Instance.currentLevel].room);
        }
        // If we are in the first room of a level, set the camera
        // to the first room's camera.
        else
        {
            GetCurrentCameraData(1);
        }

        SetCurrentPlayer(Player);

        returnSpeed = 20f;
        returnToPlayerTimer = 0f;
        accelerationValue = 1f;
        moveSpeed = 6f;

        forceBallToPlayer = false;
        
        camBallOffset.y = curCamera.GetComponent<CinemachineVirtualCamera>()
            .GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.y;
    }

    // Takes a new player as a parameter and sets the current camera's follow target
    // to the new player, and forces the ball to that players position.
    public void SetCurrentPlayer(GameObject newPlayer)
    {
        Player = newPlayer;
        curCamera.Follow = Player.transform;
        isCamFollowingPlayer = true;
        camFollowSphere.position = playerPos;

        forceBallToPlayer = true;
        camFollowSphere.transform.position = Player.transform.position;
    }
    
    // Changes the current camera depending on the main camera's animator state.
    public void GetCurrentCameraData(int newRoom)
    {
        curCamera = GameObject.FindGameObjectWithTag
            ("VCam" + newRoom.ToString()).GetComponent<CinemachineVirtualCamera>();
        
        GameObject.FindGameObjectWithTag("StateDrivenCam").GetComponent<Animator>().SetInteger("roomNum", newRoom);

        // Get the confiner data of the new camera.
        GetCurrentConfinerData();
        
        camBallOffset.y = curCamera.GetComponent<CinemachineVirtualCamera>()
            .GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.y;
    }

    // Grab all needed information about the current camera's confiner
    // by grabbing the confiner's scale values, position values, and setting
    // the follow sphere's movement bounds based on these values.
    public void GetCurrentConfinerData()
    {
        curConfiner = curCamera.GetComponent<CinemachineConfiner>();
        
        curConfinerScaleX = (curConfiner.m_BoundingVolume.transform.lossyScale.x * .5f);
        curConfinerPosX = curConfiner.m_BoundingVolume.transform.position.x;
        
        curConfinerScaleY = curConfiner.m_BoundingVolume.transform.lossyScale.y * .5f;
        curConfinerPosY = curConfiner.m_BoundingVolume.transform.position.y;

        leftXBound = (curConfinerPosX - curConfinerScaleX);
        rightXBound = (curConfinerPosX + curConfinerScaleX);
        
        leftYBound = (curConfinerPosY - curConfinerScaleY);
        rightYBound = (curConfinerPosY + curConfinerScaleY);
    }

    // Clamp the position of the camera follow sphere to the bounds of the
    // current camera's confiner.
    void ClampCameraFollowSphere()
    {
        Vector3 position = camFollowSphere.transform.position;

        position.x = Mathf.Clamp(position.x, leftXBound, rightXBound);
        position.y = Mathf.Clamp(position.y, leftYBound - camBallOffset.y, rightYBound - camBallOffset.y);
            
        camFollowSphere.transform.position = position;
    }

    // Called each frame.
    void Update()
    {
        // If the player is not moving and the sphere is not following
        // the player, clamp the follow sphere to the bounds of the 
        // current camera's confiner.
        if (isCamFollowingPlayer == false && !playerMove.IsPressed() && canUseFreeCam)
        {
            ClampCameraFollowSphere();
        }

        // If we were forcing the follow sphere to the player and it has reached the player
        // force it no longer.
        if (forceBallToPlayer && camMove.IsPressed() 
                              && Vector3.Distance(camFollowSphere.transform.position, playerPos) < 0.25f)
        {
            forceBallToPlayer = false;
        }
    }

    // Called between frames.
    void FixedUpdate()
    {
        playerPos = Player.transform.position;
        
        // If the camera follow sphere is being controlled and the player is not, set the camera target to the
        // camera follow sphere and move the sphere along a 2D plane (x, y) according to player input via arrows
        // or d-pad. Return timer remains at a static value as long as the camera is being moved.
        if (camMove.IsPressed() && !playerMove.IsPressed() && !forceBallToPlayer)
        {
            if (isCamFollowingPlayer)
            {
                curCamera.Follow = transform;

                isCamFollowingPlayer = false;
            }
            
            returnToPlayerTimer = 1.5f;

            moveDirection = camMove.ReadValue<Vector2>().normalized * moveSpeed;
            
                camFollowSphere.velocity = new Vector3(moveDirection.x * accelerationValue,
                    moveDirection.y * accelerationValue, camFollowSphere.velocity.z);
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

            transform.position = playerPos;
        }
    }

    private void EnablePlayerControls()
    {
        playerMove = playerControls.Movement.Move;
        playerMove.Enable();
        
        camMove = playerControls.Movement.CamMove;
        camMove.Enable();

        cloneButton = playerControls.SummonClone.SwitchPlaces;
        cloneButton.Enable();
    }

    private void OnDisable()
    {
        playerMove.Disable();
        camMove.Disable();
        cloneButton.Disable();
    }
}
