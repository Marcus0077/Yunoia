using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStopPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        GameManager.Instance.DisableInput();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
