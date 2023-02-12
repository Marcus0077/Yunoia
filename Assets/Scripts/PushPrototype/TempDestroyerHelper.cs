using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDestroyerHelper : MonoBehaviour
{
    // Script added by tempdestroyer to autmatically decrement counter in tempdestroyer
    public TempDestroyer counter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnDestroy()
    {
        if (counter != null)
            counter.count--;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
