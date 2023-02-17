using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleMeasure : MonoBehaviour
{
    public float weight = 0, minionScale = 1, cloneWeight = 2, facelessWeight = 1, boxWeight = 1;
    int minionCount;
    BasicMovement player;
    BasicMovement clone;
    bool cloneOn;
    // Start is called before the first frame update
    void Start()
    {
        player = null;
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            player = collider.gameObject.GetComponent<BasicMovement>();
            minionCount = player.attachedMinionCount;
            weight += 1 + minionCount * minionScale;
        }
        else if(collider.tag == "Clone")
        {
            clone = collider.gameObject.GetComponent<BasicMovement>();
            cloneOn = true;
            weight += cloneWeight;
        }
        else if (collider.tag == "Faceless")
        {
            weight += facelessWeight;
        }
        else if (collider.tag == "BoxScale")
        {
            weight += boxWeight;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            player = null;
            weight -= 1 + minionCount * minionScale;
        }
        else if (collider.tag == "Clone") //check if clone despawns?
        {
            clone = null;
            cloneOn = false;
            weight -= cloneWeight;
        }
        else if (collider.tag == "Faceless")
        {
            weight -= facelessWeight;
        }
        else if (collider.tag == "BoxScale")
        {
            weight -= boxWeight;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            if(minionCount != player.attachedMinionCount)
            {
                weight -= (minionCount - player.attachedMinionCount) * minionScale;
                minionCount = player.attachedMinionCount;
            }
        }
        if(clone == null && cloneOn)
        {
            weight -= cloneWeight;
            cloneOn = false;
        }
    }
}
