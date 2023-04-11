using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class AngerMooseRun : MonoBehaviour
{
    private Animator mooseAnimator;
    private GameManager gameManager;
    public CinemachineVirtualCamera VCam1;
    private IEnumerator killCoroutine;
    
    // Start is called before the first frame update
    void Start()
    {
        mooseAnimator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        
        mooseAnimator.SetFloat("Speed", 8);
        
        killCoroutine = KillMoose();
        StartCoroutine(killCoroutine);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, 
            new Vector3(transform.position.x + 0.4f, transform.position.y, transform.position.z), 1f);
    }

    private IEnumerator KillMoose()
    {
        Debug.Log("bruh moment");
        
        gameManager.DisableInput();
        
        VCam1.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = new Vector3(0, 8f, 18);
        VCam1.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = 30;
        
        yield return new WaitForSeconds(4f);
        
        VCam1.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = new Vector3(0, 0, 0);
        VCam1.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = 12;
            
        gameManager.EnableInput();
            
        Destroy(this.GameObject());
    }
}
