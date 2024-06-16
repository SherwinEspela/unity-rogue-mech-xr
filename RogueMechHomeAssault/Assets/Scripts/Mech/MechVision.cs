using UnityEngine;
using UnityEngine.Events;

public class MechVision : MonoBehaviour
{
    [SerializeField] Camera cameraVision;
    [SerializeField] Transform visionStart;
    [SerializeField] Vector3 visionEnd;
    [SerializeField] Vector3 boxRaycastSize;
    [SerializeField] Vector3 boxPlayerHitSize;
    [SerializeField] int boxRaycastDistance = 5;

    private Plane[] planes;
    private bool isPlayerInSight = false;
    private bool didHit = false;
    private RaycastHit hit;

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
            isPlayerInSight = GeometryUtility.TestPlanesAABB(planes, playerBounds);
        }
    }

    void FixedUpdate()
    {
        if (!isPlayerInSight) return;

        didHit = Physics.Raycast(visionStart.position, transform.forward, out hit, boxRaycastDistance);
        if (didHit)
        {
            var tagIsPlayer = hit.transform.tag.ToLower().Contains("player");
            if (tagIsPlayer)
            {
                Debug.Log("Hit : " + hit.transform.tag);
            } else
            {
                Debug.Log("hitting other objects.....");
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Gizmos.DrawWireCube(visionStart.position, boxRaycastSize);

        if (isPlayerInSight)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(visionStart.position, visionStart.forward * boxRaycastDistance);

            if (didHit)
            {
                var tagIsPlayer = hit.transform.tag.ToLower().Contains("player");
                if (tagIsPlayer)
                {
                    Gizmos.DrawWireCube(Player.transform.position, boxPlayerHitSize);
                }
            }
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(visionStart.position, visionStart.forward * boxRaycastDistance);
        }

        //if (didHit)
        //{
        //    var tagIsPlayer = hit.transform.tag.ToLower().Contains("player");

        //    if (tagIsPlayer)
        //    {
        //        Gizmos.color = Color.red;
        //        Gizmos.DrawRay(visionStart.position, transform.forward * hit.distance);
        //    }
        //}
        //else
        //{
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawRay(transform.position, transform.forward * boxRaycastDistance);
        //    //Gizmos.DrawWireCube(transform.position + transform.forward * boxRaycastDistance, boxRaycastSize);
        //}
    }
}
