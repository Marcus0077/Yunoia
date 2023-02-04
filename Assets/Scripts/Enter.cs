using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enter : MonoBehaviour
{
    public GameObject turnPlatform;

private void OnTriggerEnter()
{
    turnPlatform.SetActive(true);
}

}
