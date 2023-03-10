using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Save checkpoint position if player touches checkpoint
    void OnTriggerEnter(Collider collision) 
    {
        GameObject hit = collision.GameObject();

        if (hit.tag == "Player")
        {
            DataManager.gameData.checkpointed = true;
            Debug.Log(GameManager.Instance.currentLevel);
            var checkpointData = new CheckpointData();
            checkpointData.room = GameObject.FindWithTag("StateDrivenCam").GetComponent<Animator>().GetInteger("roomNum");
            checkpointData.position = hit.transform.position;
            GameManager.Instance.SetCheckpoint(checkpointData);
            //DataManager.gameData.checkpointDatas[GameManager.Instance.currentLevel].room = GameObject.FindWithTag("StateDrivenCam").GetComponent<Animator>().GetInteger("roomNum");
            //DataManager.gameData.checkpointDatas[GameManager.Instance.currentLevel].position = hit.transform.position;
            //DataManager.gameData.level = GameObject.FindWithTag("StateDrivenCam").GetComponent<Animator>().GetInteger("roomNum");
            //DataManager.gameData.position = hit.transform.position;
            GameObject.FindWithTag("Save Icon").GetComponent<Animator>().SetTrigger("Saving");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}