using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool ghost = false, menuCursor = false;
    [Range(0.0f, 1.0f)]
    public float sensitivity = 1;
    [SerializeField]
    GameObject player, ghostObj;
    GameObject spawnedGhost;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        player = GameObject.FindGameObjectWithTag("Player");
        sensitivity = PlayerPrefs.GetFloat("Sensitivity");
    }

    public void ToggleGhost()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if(player == null)
                return;
        }
        ghost = !ghost;
        if(ghost)
        {
            Time.timeScale = 0;
            BlockPlayerInput();
            spawnedGhost = Instantiate(ghostObj, player.transform.position, player.transform.rotation);
        }
        else
        {
            Time.timeScale = 1;
            EnablePlayerInput();
            Destroy(spawnedGhost);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void EnablePlayerInput()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
                return;
        }
        player.GetComponent<BasicMovement>().playerControls.Enable();
        player.GetComponent<AbilityPush>().pushControls.Enable();
        player.GetComponent<Grapple>().grappleControls.Enable();
        player.GetComponent<SummonClone>().summonControls.Enable();
        player.GetComponent<PlayerInteractions>().playerControls.Enable();
        GameObject clone = GameObject.FindGameObjectWithTag("Clone");
        if (clone != null)
        {
            clone.GetComponent<BasicMovement>().playerControls.Enable();
            clone.GetComponent<AbilityPush>().pushControls.Enable();
            clone.GetComponent<Grapple>().grappleControls.Enable();
            clone.GetComponent<CloneInteractions>().playerControls.Enable();
        }
        GameObject pause = GameObject.FindGameObjectWithTag("Pause");
        if (pause != null)
        {
            pause.GetComponent<PauseMenu>().playerControls.Enable();
        }
    }

    public void BlockPlayerInput()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
                return;
        }
        player.GetComponent<BasicMovement>().playerControls.Disable();
        player.GetComponent<AbilityPush>().pushControls.Disable();
        player.GetComponent<Grapple>().grappleControls.Disable();
        player.GetComponent<SummonClone>().summonControls.Disable();
        player.GetComponent<PlayerInteractions>().playerControls.Disable();
        GameObject clone = GameObject.FindGameObjectWithTag("Clone");
        if (clone != null)
        {
            clone.GetComponent<BasicMovement>().playerControls.Disable();
            clone.GetComponent<AbilityPush>().pushControls.Disable();
            clone.GetComponent<Grapple>().grappleControls.Disable();
            clone.GetComponent<CloneInteractions>().playerControls.Disable();
        }
        GameObject pause = GameObject.FindGameObjectWithTag("Pause");
        if(pause != null)
        {
            pause.GetComponent<PauseMenu>().playerControls.Disable();
        }
    }

    void OnEnabled()
    {
        
    }

    void OnDisable()
    {
        
    }

    void FixedUpdate()
    {
        
    }
}