using UnityEngine;

public class EdgeScanner : MonoBehaviour
{
    [SerializeField] LayerMask layersToIgnore;
    [SerializeField] float radius = 0.3f;
    [SerializeField] float offsetY = 0.75f;
    [SerializeField] float raycastDistance = 1.0f;

    public void Scan(GameObject spawnPoint)
    {
        var pos = spawnPoint.transform.position;
        var newPos = new Vector3(pos.x, pos.y + offsetY, pos.z);
        transform.position = newPos;

        var hits = Physics.SphereCastAll(transform.position, radius, transform.TransformDirection(Vector3.down), raycastDistance, layersToIgnore);
        if (hits.Length < 5)
        {
            spawnPoint.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
