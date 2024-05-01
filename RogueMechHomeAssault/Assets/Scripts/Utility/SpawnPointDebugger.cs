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
    [SerializeField] LayerMask layerMaskToIgnore;

    private void Start()
    {
        grid.cellSize = Vector3.one * gridCellSize; //new Vector3(gridCellSize, gridCellSize, gridCellSize);
        grid.cellGap = Vector3.one * gridCellGap; //new Vector3(gridCellGap, gridCellGap, gridCellGap);

        VisualizeGridLocations();
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

    private void AddDebugSphere(Vector3 pos)
    {
        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = Vector3.one * debugSphereSize;
        sphere.GetComponent<Renderer>().material.color = Color.red;
        var bc = sphere.GetComponent<BoxCollider>();
        if (bc) bc.enabled = false;
        sphere.transform.position = pos;
    }
}
