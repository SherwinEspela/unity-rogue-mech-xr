using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointDebugger : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] float gridCellSize = 0.1f;
    [SerializeField] float gridCellGap = 0.02f;
    [SerializeField] int gridStart = -50;
    [SerializeField] int gridEnd = 50;
    [SerializeField] Color colorDebugSphere = Color.red;
    [SerializeField] float debugSphereSize = 0.03f;
    [SerializeField] Transform spawnPointsContainer;
    [SerializeField] LayerMask layerMaskToIgnore;

    private List<GameObject> debugSpheres;

    private void Start()
    {
        debugSpheres = new List<GameObject>();
        grid.cellSize = Vector3.one * gridCellSize;
        grid.cellGap = Vector3.one * gridCellGap;

        GenerateSpawnPoints();
        SelectPointsInsideTheRoom();
    }

    private void GenerateSpawnPoints()
    {
        if (grid == null) grid = GetComponent<Grid>();

        for (int i = gridStart; i < gridEnd; i++)
        {
            for (int j = gridStart; j < gridEnd; j++)
            {
                Vector3Int cell = new Vector3Int(i, 0, j);
                Vector3 worldPos = grid.CellToWorld(cell);
                AddDebugSphere(worldPos);
            }
        }
    }

    private void SelectPointsInsideTheRoom()
    {
        if (debugSpheres.Count == 0)
        {
            Debug.LogError("There are no debug spheres.");
        }

        foreach (var ds in debugSpheres)
        {
            bool isInside = IsInsideRoom(ds.transform.position);
            
            // show only the debug sphere
            // that are inside the room
            ds.SetActive(isInside);
        }
    }

    private bool IsInsideRoom(Vector3 position)
    {
        bool isHittingTheFloor = Physics.Raycast(position, Vector3.down, Mathf.Infinity);
        return isHittingTheFloor;
    }

    private void AddDebugSphere(Vector3 pos)
    {
        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = Vector3.one * debugSphereSize;
        sphere.GetComponent<Renderer>().material.color = colorDebugSphere;
        var bc = sphere.GetComponent<BoxCollider>();
        if (bc) bc.enabled = false;
        pos.y = gridCellSize;
        sphere.transform.position = pos;
        sphere.name = "DebugSphere";
        sphere.transform.SetParent(this.transform);
        debugSpheres.Add(sphere);
    }
}
