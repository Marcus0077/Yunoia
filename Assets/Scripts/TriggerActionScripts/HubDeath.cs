using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class HubDeath : MonoBehaviour
{
    [SerializeField]
    Transform cameraPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var checkpointData = new CheckpointData();
            checkpointData.room = 1;
            checkpointData.position = new Vector3(3.218f, 43.83f, 11.162779f);
            GameManager.Instance.SetCheckpoint(checkpointData);
            GameObject.FindWithTag("VCam1").GetComponent<CinemachineVirtualCamera>().Follow = cameraPoint;
            other.gameObject.transform.position = new Vector3(GameManager.Instance.GetCheckpoint().position.x, GameManager.Instance.GetCheckpoint().position.y+20, GameManager.Instance.GetCheckpoint().position.z);
            GameManager.Instance.BlockPlayerInput();
            StartCoroutine(Falling(other.transform));
        }
    }

    IEnumerator Falling(Transform player)
    {
        yield return new WaitForSeconds(1.2f);
        GameManager.Instance.EnablePlayerInput();
        GameObject.FindWithTag("VCam1").GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
