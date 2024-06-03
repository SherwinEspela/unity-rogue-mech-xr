using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class RoomManager : MonoBehaviour
{
    [SerializeField] OVRSceneManager sceneManager;
    [SerializeField] Grid grid;
    [SerializeField] BoxCollider roomBoxCollider;
    [SerializeField] SceneDebugger sceneDebugger;
    [SerializeField] bool isDebugging = false;

    [SerializeField] float gridCellSize = 0.1f;
    [SerializeField] float gridCellGap = 0.02f;
    [SerializeField] int gridStart = -50;
    [SerializeField] int gridEnd = 50;
    [SerializeField] LayerMask layerMaskFurniture;

    private OVRSceneRoom sceneRoom;

    private void Start()
    {
        grid.cellSize = new Vector3(gridCellSize, gridCellSize, gridCellSize);
        grid.cellGap = new Vector3(gridCellGap, gridCellGap, gridCellGap);

        if (isDebugging)
        {
            //DebugGridComponent();
            VisualizeGridLocations();
            return;
        }

        //sceneDebugger.PrintMessage(LogMessageHelper.GetMessages("Start..."));

        if (sceneManager == null) sceneManager = GameObject.FindFirstObjectByType<OVRSceneManager>();
        sceneManager.SceneModelLoadedSuccessfully += OnSceneLoaded;

        grid = GetComponent<Grid>();
        roomBoxCollider = GetComponent<BoxCollider>();
        roomBoxCollider.isTrigger = true;
    }

    private void OnSceneLoaded()
    {
        //sceneDebugger.PrintMessage(LogMessageHelper.GetMessages("OnSceneLoaded..."));

        sceneRoom = GameObject.FindFirstObjectByType<OVRSceneRoom>();
        if (sceneRoom == null) return;

        //sceneDebugger.PrintMessage(LogMessageHelper.GetMessages("scene room is not null..."));

        //// setup room box collider
        float height = sceneRoom.Walls[0].Height;
        float width = sceneRoom.Floor.Width;
        float depth = sceneRoom.Floor.Height;

        roomBoxCollider.size = new Vector3(width, depth, height);
        roomBoxCollider.center = new Vector3(0, 0, height / 2);

        DebugAvailableLocations();
    }

    public bool IsInsidePlayAreaBoundary(Vector3 position)
    {
        position.y = 0;
        var result = OVRManager.boundary.TestPoint(position, OVRBoundary.BoundaryType.PlayArea);
        float distance = result.ClosestDistance;
        const float safeDistance = 0.1f;
        return distance <= safeDistance;
    }

    private bool IsInsideFurniture(Vector3 position)
    {
        bool isInFurniture = Physics.CheckBox(position, grid.cellSize, transform.rotation, layerMaskFurniture);
        //bool isBelowFurniture = Physics.Raycast(position, Vector3.up, Mathf.Infinity, layerMaskFurniture);
        return isInFurniture;
    }

    //public bool IsInsideFurniture(Vector3 position)
    //{
    //    return true;
    //}

    public List<Vector3> GetAvailableSpawnLocations()
    {
        if (grid == null) grid = GetComponent<Grid>();

        var availablelocations = new List<Vector3>();

        for (int i = gridStart; i < gridEnd; i++)
        {
            for (int j = gridStart; j < gridEnd; j++)
            {
                Vector3Int cell = new Vector3Int(i, 0, j);
                Vector3 worldPos = grid.CellToWorld(cell);

                Vector3 floorOffset = worldPos;
                floorOffset.y += 0.5f;

                bool isInsidePlayAreaBoundary = IsInsidePlayAreaBoundary(worldPos);

                if (isInsidePlayAreaBoundary)
                {
                    availablelocations.Add(worldPos);
                }
            }
        }

        return availablelocations;
    }

    private void DebugGridComponent()
    {
        if (grid == null) grid = GetComponent<Grid>();
        var availablelocations = new List<Vector3>();

        for (int i = gridStart; i < gridEnd; i++)
        {
            for (int j = gridStart; j < gridEnd; j++)
            {
                Vector3Int cell = new Vector3Int(i, 0, j);
                Vector3 worldPos = grid.CellToWorld(cell);

                bool isInsideFurniture = IsInsideFurniture(worldPos);
                if (isInsideFurniture)
                {
                    AddDebugSphere(worldPos);
                }
            }
        }
    }

    private void VisualizeGridLocations()
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

    //private void VisualizePlayAreaPoints()
    //{
    //    var positions = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);

    //    foreach (var pos in positions)
    //    {
    //        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    //        sphere.transform.localScale = Vector3.one * 0.02f;
    //        sphere.GetComponent<Renderer>().material.color = Color.cyan;
    //        sphere.transform.position = pos;
    //    }
    //}

    private void DebugAvailableLocations()
    {
        //sceneDebugger.PrintMessage(LogMessageHelper.GetMessages("DebugAvailableLocations..."));

        List<Vector3> availableLocations = GetAvailableSpawnLocations();
        if (availableLocations.Count == 0) return;

        string countMessage = $"availableLocations count ==== {availableLocations.Count}";
        //sceneDebugger.PrintMessage(LogMessageHelper.GetMessages(countMessage));

        Gizmos.color = Color.green;
        foreach (Vector3 pos in availableLocations)
        {
            AddDebugSphere(pos);
        }
    }

    private void AddDebugSphere(Vector3 pos)
    {
        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = Vector3.one * 0.03f;
        sphere.GetComponent<Renderer>().material.color = Color.red;
        var bc = sphere.GetComponent<BoxCollider>();
        if (bc) bc.enabled = false;
        sphere.transform.position = pos;
    }
}
