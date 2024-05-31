using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSceneManager : MonoBehaviour
{
    [SerializeField] SpawnPointGenerator spawnPointDebugger;
    [SerializeField] Pistol[] testWeapons;
    [SerializeField] NavMeshBaker navMeshBaker;
    [SerializeField] PoolManager poolManager;
    [SerializeField] FurnitureEdgeFinder furnitureSpawnPointsEdgeChecker;

    private Mech currentMech;

    void Start()
    {
        spawnPointDebugger.OnSpawnPointPlacementCompleted += HandleSpawnPointPlacementCompleted;
        navMeshBaker.OnNavMeshBakingComplete += HandleNavMeshBakingCompleted;
    }

    private void HandleSpawnPointPlacementCompleted()
    {
        PlaceWeapons();

        currentMech = poolManager.SpawnMech();
        currentMech.OnDestinationReached += HandleMechDestinationReached;

        var randomPos = spawnPointDebugger.RandomSpawnPoint;
        currentMech.transform.position = randomPos;
        SetNewDestination();

        // FIXME: Physics Raycasting on instantiated game objects only
        // works when invoking it is delayed.
        Invoke("ScanEdges", 0.001f);
    }

    private void ScanEdges()
    {
        var furniturePoints = spawnPointDebugger.SpawnPointsOnFurniture;
        if (furniturePoints.Count > 0)
        {
            furnitureSpawnPointsEdgeChecker.FindEdges(furniturePoints);
        }
    }

    private void HandleNavMeshBakingCompleted()
    {
    }

    private void PlaceWeapons()
    {
        if (testWeapons.Length == 0) return;

        foreach (var tw in testWeapons)
        {
            tw.transform.position = spawnPointDebugger.RandomSpawnPoint;
        }
    }

    private void HandleMechDestinationReached()
    {
        Invoke("SetNewDestination", Random.Range(2.0f, 5.0f));
    }

    private void SetNewDestination()
    {
        var destination = spawnPointDebugger.RandomSpawnPoint;
        currentMech.MoveTo(destination);
    }
}
