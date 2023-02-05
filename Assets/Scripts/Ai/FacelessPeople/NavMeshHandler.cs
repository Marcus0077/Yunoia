using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshHandler : MonoBehaviour
{
    public NavMeshSurface[] surfaces;
    public NavMeshSurface[] scales;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void BuildScaleNavMesh()
    {
        for (int i = 0; i < surfaces.Length; i++)
        {
            scales[i].BuildNavMesh();
        }
    }

    public void BuildAllNavMesh()
    {
        for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //for (int i = 0; i < surfaces.Length; i++) 
        //{
        //    surfaces [i].BuildNavMesh ();
        //    Debug.Log("a");
        //}    
    }
}
