using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSceneManager : MonoBehaviour
{
    [SerializeField] SpawnPointGenerator spawnPointGenerator;
    [SerializeField] Pistol[] testWeapons;
    [SerializeField] NavMeshBaker navMeshBaker;
    [SerializeField] PoolManager poolManager;
    [SerializeField] FurnitureEdgeFinder furnitureEdgeFinder;
    [SerializeField] EnemySpawner enemySpawner;

    void Start()
    {
        spawnPointGenerator.OnSpawnPointPlacementCompleted += HandleSpawnPointPlacementCompleted;
        navMeshBaker.OnNavMeshBakingComplete += HandleNavMeshBakingCompleted;
        furnitureEdgeFinder.OnFurnitureEdgesFound += HandleFurnitureEdgesFound;
    }

    private void HandleSpawnPointPlacementCompleted()
    {
        //PlaceWeapons();
        enemySpawner.SetSpawnPoints(spawnPointGenerator.SpawnPoints);

        // FIXME: Physics Raycasting on instantiated game objects only
        // works when invoking it is delayed.
        Invoke("ScanEdges", 0.001f);
    }

    private void ScanEdges()
    {
        var furniturePoints = spawnPointGenerator.SpawnPointsOnFurniture;
        if (furniturePoints.Count > 0)
        {
            furnitureEdgeFinder.FindEdges(furniturePoints);
        }
    }

    private void HandleNavMeshBakingCompleted()
    {
    }

    private void HandleFurnitureEdgesFound()
    {
        enemySpawner.SpawnEnemy();
    }

    private void PlaceWeapons()
    {
        if (testWeapons.Length == 0) return;

        foreach (var tw in testWeapons)
        {
            tw.transform.position = spawnPointGenerator.RandomSpawnPoint;
        }
    }
}
