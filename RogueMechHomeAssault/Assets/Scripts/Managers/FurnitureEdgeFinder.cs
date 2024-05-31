using System.Collections.Generic;
using UnityEngine;

public class FurnitureEdgeFinder : MonoBehaviour
{
    [SerializeField] EdgeScanner edgeScanner;
    [SerializeField] GameObject[] furnitures;

    public void FindEdges(List<GameObject> furnitureSpawnPoints)
    {
        if (!edgeScanner || furnitureSpawnPoints.Count == 0) return;

        foreach (var sp in furnitureSpawnPoints)
        {
            edgeScanner.Scan(sp);
        }
    }
}
