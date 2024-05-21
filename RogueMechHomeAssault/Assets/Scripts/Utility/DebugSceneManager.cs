using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSceneManager : MonoBehaviour
{
    [SerializeField] SpawnPointDebugger spawnPointDebugger;
    [SerializeField] Pistol[] testWeapons;

    void Start()
    {
        spawnPointDebugger.OnSpawnPointPlacementCompleted += HandleSpawnPointPlacementCompleted;
    }

    private void HandleSpawnPointPlacementCompleted()
    {
        Debug.Log("HandleSpawnPointPlacementCompleted....");
        PlaceWeapons();
    }

    private void PlaceWeapons()
    {
        if (testWeapons.Length == 0) return;

        foreach (var tw in testWeapons)
        {
            tw.transform.position = spawnPointDebugger.RandomSpawnPoint;
        }
    }
}
