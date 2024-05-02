using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnPointDebugger : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] float gridCellSize = 0.1f;
    [SerializeField] float gridCellGap = 0.02f;
    [SerializeField] int gridStart = -50;
    [SerializeField] int gridEnd = 50;
    [SerializeField] Color colorDebugSphere = Color.red;
    [SerializeField] float debugSphereSize = 0.03f;
    [SerializeField] LayerMask layerMaskFurniture;
    [SerializeField] LayerMask layerMaskFloor;
    [SerializeField] LayerMask layerMaskTrimmer;
    [SerializeField] Material materialTrimmer;

    private List<GameObject> debugSpheres;
    private List<GameObject> spawnPointsFloor;
    private List<GameObject> spawnPointsFurniture;
    private bool canStartPlacingPointsOnSurfaces = false;

    private void Start()
    {
        debugSpheres = new List<GameObject>();
        spawnPointsFloor = new List<GameObject>();
        spawnPointsFurniture = new List<GameObject>();
        grid.cellSize = Vector3.one * gridCellSize;
        grid.cellGap = Vector3.one * gridCellGap;

        GenerateSpawnPoints();
        SelectPointsInsideTheRoom();
    }

    private void FixedUpdate()
    {
        PlacePointsOnSurfaces();
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
        canStartPlacingPointsOnSurfaces = true;
    }

    private void PlacePointsOnSurfaces()
    {
        if (canStartPlacingPointsOnSurfaces)
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

                    if (hit.transform.tag == "TagFloor")
                    {
                        ds.GetComponent<Renderer>().material.color = Color.blue;
                        spawnPointsFloor.Add(ds);
                    }
                    else if (hit.transform.tag == "TagFurniture")
                    {
                        ds.GetComponent<Renderer>().material.color = Color.green;
                        spawnPointsFurniture.Add(ds);
                    }
                }
            }

            canStartPlacingPointsOnSurfaces = false;

            TrimPointsOnFloor();
        }
    }

    private void TrimPointsOnFloor()
    {
        var floor = GameObject.FindGameObjectWithTag("TagFloor");
        if (!floor) return;
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.GetComponent<Renderer>().material = materialTrimmer;
        //cube.layer = layerMaskTrimmer;
        var floorMesh = floor.gameObject.GetComponent<MeshFilter>().mesh;

        if (!floorMesh) return;
        var floorSize = floorMesh.bounds.size;
        var floorScaleX = floor.transform.localScale.x;
        float floorPadding = 0.5f;
        var scaleX = floorSize.x * floorScaleX - floorPadding;
        var scaleZ = floorSize.z * floorScaleX - floorPadding;
        cube.transform.localScale = Vector3.one * 3f; // new Vector3(scaleX, 1f, scaleZ);

        foreach (var sp in spawnPointsFloor)
        {
            bool isInsideCube = Physics.CheckBox(sp.transform.position, grid.cellSize / 2f, transform.rotation, layerMaskTrimmer);
            if (!isInsideCube)
            {
                //spawnPointsFloor.Remove(sp);
                DestroyImmediate(sp);
            }
        }
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
