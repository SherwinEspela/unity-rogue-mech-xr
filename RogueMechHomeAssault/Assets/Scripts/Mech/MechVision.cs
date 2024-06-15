using UnityEngine;
using UnityEngine.Events;

public class MechVision : MonoBehaviour
{
    [SerializeField] Camera cameraVision;

    private Plane[] planes;

    public PlayerCharacter Player { get; set; }
    public UnityAction OnPlayerSeen;

    private void Update()
    {
        UpdateVision();
    }

    private void UpdateVision()
    {
        if (Player && cameraVision)
        {
            planes = GeometryUtility.CalculateFrustumPlanes(cameraVision);
            var playerBounds = Player.gameObject.GetComponent<Collider>().bounds;
            if (GeometryUtility.TestPlanesAABB(planes, playerBounds))
            {
                OnPlayerSeen?.Invoke();
            }
        }
    }
}
