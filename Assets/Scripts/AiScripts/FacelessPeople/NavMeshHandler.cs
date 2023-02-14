using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshHandler : MonoBehaviour
{
    public NavMeshSurface scales;

    public void BuildAllNavMesh()
    {
        scales.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        scales.BuildNavMesh();
    }
}
