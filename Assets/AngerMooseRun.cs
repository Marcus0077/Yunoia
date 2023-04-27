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
        this.transform.position = new Vector3(-263.835571f,1.400056f,86.4534607f);
        
        mooseAnimator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();

        Debug.Log("Current Level: " + gameManager.currentLevel);
        
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<BasicMovement>().curRoom == 1)
        {
            mooseAnimator.SetFloat("Speed", 8);

            killCoroutine = KillMoose();
            StartCoroutine(killCoroutine);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, 
            new Vector3(transform.position.x + 0.4f, transform.position.y, transform.position.z), 1f);
    }

    private IEnumerator KillMoose()
    {
        gameManager.DisableInput();
        
        VCam1.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = new Vector3(0, 12f, 18);
        VCam1.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = 30;
        
        yield return new WaitForSeconds(4f);

        float duration = 1.5f;
        float timePassed = 0f;

        while (timePassed < duration)
        {
            float t = timePassed / duration;
            
            float y = Mathf.Lerp(12f, 3.5f, t);
            float z = Mathf.Lerp(18f, 0f, t);
            float dis = Mathf.Lerp(30f, 12f, t);

            VCam1.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = 
                new Vector3(0, y, z);
            VCam1.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = dis;

            timePassed += Time.deltaTime;

            yield return null;
        }

        FindObjectOfType<LimitedMovementCam>().GetCurrentCameraData(1);
            
        gameManager.EnableInput();
            
        Destroy(this.GameObject());
    }
}
