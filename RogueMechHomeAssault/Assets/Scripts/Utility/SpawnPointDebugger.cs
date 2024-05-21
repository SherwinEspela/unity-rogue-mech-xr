using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnPointDebugger : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] GameObject prefabCubeTrimmer;
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
    [SerializeField] float trimPaddingFloor = 0.5f;
    [SerializeField] float trimPaddingFurniture = 0.5f;

    private List<GameObject> debugSpheres;
    private List<GameObject> spawnPointsFloor;
    private List<GameObject> spawnPointsFurniture;
    private List<Vector3> spawnPoints;
    private bool canStartPlacingPointsOnSurfaces = false;

    private const string TAG_FLOOR = "TagFloor";
    private const string TAG_FURNITURE = "TagFurniture";

    public Vector3 RandomSpawnPoint
    {
        get { return spawnPoints[Random.Range(0, spawnPoints.Count - 1)]; }
    }

    public UnityAction OnSpawnPointPlacementCompleted;

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

                    if (hit.transform.tag == TAG_FLOOR)
                    {
                        ds.GetComponent<Renderer>().material.color = Color.blue;
                        spawnPointsFloor.Add(ds);
                    }
                    else if (hit.transform.tag == TAG_FURNITURE)
                    {
                        ds.GetComponent<Renderer>().material.color = Color.green;
                        spawnPointsFurniture.Add(ds);
                    }
                }
            }

            canStartPlacingPointsOnSurfaces = false;

            TrimPointsOnFloor();
            TrimPointsOnFurnitures();
        }
    }

    private void TrimPointsOnFloor()
    {
        var floor = GameObject.FindGameObjectWithTag(TAG_FLOOR);
        if (!floor) return;
        
        var cube = CreateTrimmerFromRoomElement(floor);
        
        var remainingSpawnPoints = new List<GameObject>();
        foreach (var sp in spawnPointsFloor)
        {
            bool isInsideCube = IsInsideTrimmer(sp.transform.position);
            if (isInsideCube)
            {
                remainingSpawnPoints.Add(sp);
            } else
            {
                DestroyImmediate(sp);
            }
        }

        spawnPointsFloor.Clear();
        spawnPointsFloor = remainingSpawnPoints;

        DestroyImmediate(cube);

        var furnitures = GameObject.FindGameObjectsWithTag(TAG_FURNITURE);
        if (furnitures.Length > 0)
        {
            foreach (var furniture in furnitures)
            {
                TrimFloorPointsCloseToFurniture(furniture);
            }
        }
    }

    private void TrimFloorPointsCloseToFurniture(GameObject furniture)
    {
        var furnitureMesh = furniture.gameObject.GetComponent<MeshFilter>().mesh;
        if (!furnitureMesh) return;

        var cube = CreateTrimmerFromRoomElement(furniture);

        var remainingSpawnPoints = new List<GameObject>();
        foreach (var sp in spawnPointsFloor)
        {
            bool isInsideCube = IsInsideTrimmer(sp.transform.position);
            if (isInsideCube)
            {
                DestroyImmediate(sp);
            }
            else
            {
                remainingSpawnPoints.Add(sp);
            }
        }

        spawnPointsFloor.Clear();
        spawnPointsFloor = remainingSpawnPoints;

        DestroyImmediate(cube);
    }

    private void TrimPointsOnFurnitures()
    {
        var furnitures = GameObject.FindGameObjectsWithTag(TAG_FURNITURE);
        if (furnitures.Length == 0) return;

        var trimmers = new List<GameObject>();
        foreach (var furniture in furnitures)
        {
            var trimmer = CreateTrimmerOnFurniture(furniture);
            trimmers.Add(trimmer);
        }

        var remainingSpawnPoints = new List<GameObject>();
        foreach (var sp in spawnPointsFurniture)
        {
            bool isInsideCube = IsInsideTrimmer(sp.transform.position);
            if (isInsideCube)
            {
                remainingSpawnPoints.Add(sp);
            }
            else
            {
                DestroyImmediate(sp);
            }
        }

        spawnPointsFurniture.Clear();
        spawnPointsFurniture = remainingSpawnPoints;

        foreach (var trimmer in trimmers)
        {
            DestroyImmediate(trimmer);
        }

        spawnPoints = new List<Vector3>();
        foreach (var go in spawnPointsFloor)
        {
            spawnPoints.Add(go.transform.position);
        }

        foreach (var go in spawnPointsFurniture)
        {
            spawnPoints.Add(go.transform.position);
        }

        OnSpawnPointPlacementCompleted?.Invoke();
    }

    private bool IsInsideTrimmer(Vector3 position)
    {
        return Physics.CheckBox(position, grid.cellSize / 2f, transform.rotation, layerMaskTrimmer);
    }

    private GameObject CreateTrimmerFromRoomElement(GameObject roomElement)
    {
        bool isFurniture = roomElement.tag == TAG_FURNITURE;
        float padding = isFurniture ? trimPaddingFloor : -trimPaddingFloor;
        return CreateTrimmer(roomElement, padding);
    }
    
    private GameObject CreateTrimmerOnFurniture(GameObject furniture)
    {
        return CreateTrimmer(furniture, -trimPaddingFurniture, false);
    }

    private GameObject CreateTrimmer(GameObject roomElement, float padding, bool isOnFloor = true)
    {
        var mesh = roomElement.gameObject.GetComponent<MeshFilter>().mesh;
        if (!mesh) return null;

        var roomElementSize = mesh.bounds.size;
        var roomElementScaleX = roomElement.transform.localScale.x;
        var roomElementScaleZ = roomElement.transform.localScale.z;

        var scaleX = roomElementSize.x * roomElementScaleX + padding;
        var scaleZ = roomElementSize.z * roomElementScaleZ + padding;

        var cube = Instantiate(prefabCubeTrimmer);
        cube.GetComponent<Renderer>().material = materialTrimmer;

        if (isOnFloor)
        {
            cube.transform.position = roomElement.transform.position;
        }
        else
        {
            var roomElementPos = roomElement.transform.position;
            roomElementPos.y += (roomElementSize.y / 2f);
            cube.transform.position = roomElementPos;
        }

        cube.transform.localScale = new Vector3(scaleX, 1f, scaleZ);
        cube.AddComponent<BoxCollider>();

        return cube;
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
