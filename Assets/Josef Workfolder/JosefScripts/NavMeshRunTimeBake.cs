using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using NavMeshPlus.Components;

public class NavMeshRunTimeBake : MonoBehaviour
{
    NavMeshSurface surface;
    // Start is called before the first frame update
    void Start()
    {
        surface = GetComponent<NavMeshSurface>();


        BakeNavMesh();
    }

    void BakeNavMesh()
    {
        surface.BuildNavMesh();
    }




    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Bake");
            BakeNavMesh();
        }
    }
}
