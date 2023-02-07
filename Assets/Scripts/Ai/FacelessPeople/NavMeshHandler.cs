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
        for (int i = 0; i < scales.Length; i++)
        {
            List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();
            NavMeshBuilder.CollectSources(
                new Bounds(GameObject.FindGameObjectWithTag("Player").transform.position, new Vector3(1, 1, 1)),
                ~0,
                NavMeshCollectGeometry.PhysicsColliders,
                0,
                new List<NavMeshBuildMarkup>(),
                sources);
            NavMeshBuilder.UpdateNavMeshData(scales[i].navMeshData, NavMesh.GetSettingsByID(0), new List<NavMeshBuildSource>(), new Bounds(GameObject.FindGameObjectWithTag("Player").transform.position, new Vector3(20, 20, 20)));
            //Debug.Log(scales[i].sourceBounds);
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
