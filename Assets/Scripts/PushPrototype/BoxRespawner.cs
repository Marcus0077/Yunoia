using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxRespawner : MonoBehaviour
{
    Vector3 originalPos;
    Quaternion originalRot;
    GameObject origBox;
    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        originalRot = transform.rotation;
        origBox = this.gameObject;
    }

    public void Reset()
    {
        GameObject go = Instantiate(origBox, originalPos, originalRot);
        go.GetComponent<Animator>().enabled = true;
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}