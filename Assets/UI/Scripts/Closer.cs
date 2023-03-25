using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    IEnumerator RemoveIcon()
    {
        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(RemoveIcon());
    }
}
