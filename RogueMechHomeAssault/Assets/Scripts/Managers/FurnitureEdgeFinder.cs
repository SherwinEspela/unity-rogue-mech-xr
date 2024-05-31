using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FurnitureEdgeFinder : MonoBehaviour
{
    [SerializeField] EdgeScanner edgeScanner;
    [SerializeField] GameObject[] furnitures;

    public UnityAction OnFurnitureEdgesFound;

    public void FindEdges(List<GameObject> furnitureSpawnPoints)
    {
        if (!edgeScanner || furnitureSpawnPoints.Count == 0) return;

        foreach (var sp in furnitureSpawnPoints)
        {
            edgeScanner.Scan(sp);
        }

        edgeScanner.gameObject.SetActive(false);
        Destroy(edgeScanner.gameObject);

        OnFurnitureEdgesFound?.Invoke();
    }
}
