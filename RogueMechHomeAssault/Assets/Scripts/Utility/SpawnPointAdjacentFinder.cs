using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointAdjacentFinder : MonoBehaviour
{
    [SerializeField] int layerMaskFurniture = 3;
    //[SerializeField] bool allowed = false;

    private bool isFindingAdjacent = false;
    private bool isEdge = false;

    private void Awake()
    {
        layerMaskFurniture = 3;
    }

    //private void Start()
    //{
    //    if (allowed)
    //    {
    //        FindAdjacents();
    //    }
    //}

    //public void SetLayerMask(int value)
    //{
    //    layerMaskFurniture = value;
    //}

    public void FindAdjacents()
    {
        Debug.Log($"FindAdjacents {this.gameObject.name}");

        //var sp = GetComponent<SphereCollider>();
        //sp.enabled = false;
        layerMaskFurniture = 3;
        isFindingAdjacent = true;
    }

    void FixedUpdate()
    {
        if (isFindingAdjacent)
        {
            float drawDuration = 100.0f;
            float raycastDistance = 300.0f;

            //Debug.DrawRay(transform.position, Vector3.forward * 0.3f, Color.red, drawDuration);
            //Debug.DrawRay(transform.position, Vector3.back * 0.3f, Color.red, drawDuration);
            //Debug.DrawRay(transform.position, Vector3.left * 0.3f, Color.red, drawDuration);
            //Debug.DrawRay(transform.position, Vector3.right * 0.3f, Color.red, drawDuration);

            //Debug.DrawRay(transform.position, Vector3.down * 1f, Color.red, drawDuration);

            RaycastHit hitForward, hitBack, hitLeft, hitRight, hitDown;
            bool didHitForward = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitForward, raycastDistance, layerMaskFurniture);
            //bool didHitForward = Physics.SphereCast(transform.position, 0.3f, transform.forward, out hitForward, raycastDistance);

            if (didHitForward)
            {
                Debug.Log("did hit forward");
                Debug.DrawRay(transform.position, Vector3.forward * hitForward.distance, Color.red, drawDuration);
            }
            else
            {
                Debug.Log("did NOT hit forward");
                //isFindingAdjacent = false;
                //return;
            }

            //bool didHitBack = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hitBack, raycastDistance);
            //if (didHitBack)
            //{
            //    Debug.Log("did hit backward");
            //    Debug.DrawRay(transform.position, Vector3.back * hitBack.distance, Color.red, drawDuration);
            //}
            //else
            //{
            //    Debug.Log("did NOT hit backward");
            //    //isFindingAdjacent = false;
            //    //return;
            //}

            //bool didHitLeft = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hitLeft, raycastDistance, 8);
            //if (didHitLeft)
            //{
            //    Debug.Log("did hit LEFT");
            //    Debug.DrawRay(transform.position, Vector3.left * hitLeft.distance, Color.red, drawDuration);
            //}
            //else
            //{
            //    Debug.Log("did NOT hit LEFT");
            //}

            bool didHitRight = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hitRight, raycastDistance, layerMaskFurniture);
            if (didHitRight)
            {
                Debug.Log("did hit Right");
                Debug.DrawRay(transform.position, Vector3.right * hitRight.distance, Color.red, drawDuration);
            }
            else
            {
                Debug.Log("did NOT hit Right");
            }

            //bool didHitDown = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitDown, raycastDistance, layerMaskFurniture);
            //if (didHitDown)
            //{
            //    Debug.Log("did hit down");
            //    Debug.DrawRay(transform.position, Vector3.down * hitDown.distance, Color.red, drawDuration);
            //}
            //else
            //{
            //    Debug.Log("did NOT hit down");
            //}

            isFindingAdjacent = false;
        }
    }
}
