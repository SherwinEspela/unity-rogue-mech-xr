using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class RoomManager : MonoBehaviour
{
    [SerializeField] OVRSceneManager sceneManager;
    [SerializeField] OVRSceneRoom sceneRoom;
    [SerializeField] Grid grid;
    [SerializeField] BoxCollider roomBoxCollider;
    [SerializeField] SceneDebugger sceneDebugger;

    [SerializeField] bool isDebugging = false;

    private void Start()
    {
        if (sceneRoom == null)
        {
            sceneRoom = GameObject.FindFirstObjectByType<OVRSceneRoom>();
        }

        if (sceneManager == null)
        {
            sceneManager = GameObject.FindFirstObjectByType<OVRSceneManager>();
            sceneManager.SceneModelLoadedSuccessfully += OnSceneLoaded;
        }

        grid = GetComponent<Grid>();
        roomBoxCollider = GetComponent<BoxCollider>();
        roomBoxCollider.isTrigger = true;
    }

    private void OnSceneLoaded()
    {
        sceneDebugger.PrintMessage("OnSceneLoaded....");

        //if (sceneRoom == null) return;

        //// setup room box collider
        //float height = sceneRoom.Walls[0].Height;
        //float width = sceneRoom.Floor.Width;
        //float depth = sceneRoom.Floor.Height;

        //roomBoxCollider.size = new Vector3(width, depth, height);
        //roomBoxCollider.center = new Vector3(0, 0, height/2);

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

    //public bool IsInsideFurniture(Vector3 position)
    //{
    //    return true;
    //}

    public List<Vector3> GetAvailableSpawnLocations()
    {
        if (grid == null) grid = GetComponent<Grid>();

        var availablelocations = new List<Vector3>();

        const int end = 25;
        const int start = -25;

        for (int i = start; i < end; i++)
        {
            for (int j = start; j < end; j++)
            {
                Vector3Int cell = new Vector3Int(i, 0, j);
                Vector3 worldPos = grid.CellToWorld(cell);

                Vector3 floorOffset = worldPos;
                floorOffset.y += 0.5f;

                AddDebugSphere(worldPos);

                bool isInsidePlayAreaBoundary = IsInsidePlayAreaBoundary(worldPos);

                if (isInsidePlayAreaBoundary)
                {
                    availablelocations.Add(worldPos);
                }
            }
        }

        return availablelocations;
    }

    private void VisualizePlayAreaPoints()
    {
        var positions = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);

        foreach (var pos in positions)
        {
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = Vector3.one * 0.02f;
            sphere.GetComponent<Renderer>().material.color = Color.cyan;
            sphere.transform.position = pos;
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (!isDebugging) return;

        
    //}

    private void DebugAvailableLocations()
    {
        Debug.Log("DebugAvailableLocations......");
        sceneDebugger.PrintMessage("DebugAvailableLocations.....");

        List<Vector3> availableLocations = GetAvailableSpawnLocations();
        if (availableLocations.Count == 0) return;

        Debug.Log($"availableLocations count ==== {availableLocations.Count}");
        sceneDebugger.PrintMessage($"availableLocations count ==== { availableLocations.Count}");

        Gizmos.color = Color.green;
        foreach (Vector3 pos in availableLocations)
        {
            //Gizmos.DrawCube(location, grid.cellSize * .2f);

            AddDebugSphere(pos);
        }
    }

    private void AddDebugSphere(Vector3 pos)
    {
        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = Vector3.one * 0.1f;
        sphere.GetComponent<Renderer>().material.color = Color.cyan;
        var bc = sphere.GetComponent<BoxCollider>();
        bc.enabled = false;
        sphere.transform.position = pos;
    }
}
