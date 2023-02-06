using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeFaceless : MonoBehaviour
{
    Vector3 originalPos;
    Quaternion originalRot;
    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        originalRot = transform.rotation;
    }

    public void Reset()
    {
        Instantiate(this, originalPos, originalRot);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
