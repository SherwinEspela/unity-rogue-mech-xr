using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class NavMeshBaker : MonoBehaviour
{
    private List<NavMeshSurface> navMeshSurfaces;
    
    private const string TAG_FLOOR = "TagFloor";
    private const string TAG_FURNITURE = "TagFurniture";

    public UnityAction OnNavMeshBakingComplete;

    void Awake()
    {
        var floors = GameObject.FindGameObjectsWithTag(TAG_FLOOR);
        if (floors.Length > 0)
        {
            navMeshSurfaces = new List<NavMeshSurface>();

            foreach (var fl in floors)
            {
                fl.AddComponent<NavMeshSurface>();
                navMeshSurfaces.Add(fl.GetComponent<NavMeshSurface>());
            }

            var furnitures = GameObject.FindGameObjectsWithTag(TAG_FURNITURE);
            foreach (var ft in furnitures)
            {
                ft.AddComponent<NavMeshSurface>();
                navMeshSurfaces.Add(ft.GetComponent<NavMeshSurface>());
            }

            if (navMeshSurfaces.Count > 0)
            {
                foreach (var nvs in navMeshSurfaces)
                {
                    nvs.BuildNavMesh();
                }

                OnNavMeshBakingComplete?.Invoke();
            }
        }
    }
}
