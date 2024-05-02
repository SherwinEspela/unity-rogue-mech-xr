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
    [SerializeField] LayerMask layerMaskFurniture;
    [SerializeField] LayerMask layerMaskFloor;

    private List<GameObject> debugSpheres;
    private List<GameObject> debugSpheresInsideFurniture;
    private bool canUpdateFurnitureSpawnPointPositions;

    private void Start()
    {
        debugSpheres = new List<GameObject>();
        grid.cellSize = Vector3.one * gridCellSize;
        grid.cellGap = Vector3.one * gridCellGap;
        canUpdateFurnitureSpawnPointPositions = false;

        GenerateSpawnPoints();
        SelectPointsInsideTheRoom();
        canUpdateFurnitureSpawnPointPositions = true;
    }

    private void FixedUpdate()
    {
        if (canUpdateFurnitureSpawnPointPositions)
        {
            LayerMask layersToInclude = layerMaskFurniture | layerMaskFloor;

            foreach (var ds in debugSpheres)
            {
                RaycastHit hit;
                bool didHit = Physics.Raycast(
                    ds.transform.position, 
                    ds.transform.TransformDirection(Vector3.down), 
                    out hit, 
                    Mathf.Infinity, 
                    layersToInclude
                );

                if (didHit)
                {
                    ds.transform.position = hit.point;
                    ds.GetComponent<Renderer>().material.color = Color.green;
                }
            }

            canUpdateFurnitureSpawnPointPositions = false;
        }
    }

    private void GenerateSpawnPoints()
    {
        if (grid == null) grid = GetComponent<Grid>();

        for (int i = gridStart; i < gridEnd; i++)
        {
            for (int j = gridStart; j < gridEnd; j++)
            {
                Vector3Int cell = new Vector3Int(i, (int)(transform.position.y), j);
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
            return;
        }

        List<GameObject> spheresInsideRoom = new List<GameObject>();
        foreach (var ds in debugSpheres)
        {
            bool isHittingTheFloor = Physics.Raycast(ds.transform.position, Vector3.down, Mathf.Infinity, layerMaskFloor);
            if (isHittingTheFloor)
            {
                spheresInsideRoom.Add(ds);
            } else
            {
                DestroyImmediate(ds);
            }
        }

        debugSpheres.Clear();
        debugSpheres.AddRange(spheresInsideRoom);
    }

    private void PlacePointsOnTopOfFurnitures()
    {
        if (debugSpheres.Count == 0)
        {
            Debug.LogError("There are no debug spheres.");
            return;
        }

        debugSpheresInsideFurniture = new List<GameObject>();

        foreach (var ds in debugSpheres)
        {            
            bool isInFurniture = Physics.CheckBox(ds.transform.position, grid.cellSize, transform.rotation, layerMaskFurniture);
            if (isInFurniture)
            {
                ds.GetComponent<Renderer>().material.color = Color.blue;
                debugSpheresInsideFurniture.Add(ds);
            }
        }

        canUpdateFurnitureSpawnPointPositions = debugSpheresInsideFurniture.Count > 0;
    }

    private bool IsInsideRoom(Vector3 position)
    {
        bool isHittingTheFloor = Physics.Raycast(position, Vector3.down, Mathf.Infinity);
        return isHittingTheFloor;
    }

    private bool IsInsideFurniture(Vector3 position)
    {
        return Physics.CheckBox(position, grid.cellSize, transform.rotation);
    }

    private void AddDebugSphere(Vector3 pos)
    {
        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = Vector3.one * debugSphereSize;
        sphere.GetComponent<Renderer>().material.color = colorDebugSphere;
        var bc = sphere.GetComponent<BoxCollider>();
        if (bc) bc.enabled = false;
        sphere.transform.SetParent(this.transform);
        sphere.transform.position = pos;
        sphere.name = "DebugSphere";
  
        debugSpheres.Add(sphere);
    }
}
