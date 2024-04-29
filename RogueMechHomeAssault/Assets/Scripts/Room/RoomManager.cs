using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] OVRSceneManager sceneManager;
    [SerializeField] OVRSceneRoom sceneRoom;
    [SerializeField] Grid grid;
    [SerializeField] BoxCollider roomBoxCollider;

    private void Start()
    {
        if (sceneManager == null)
        {
            sceneManager = GameObject.FindFirstObjectByType<OVRSceneManager>();
            sceneManager.SceneModelLoadedSuccessfully += OnSceneLoaded;
        }

        grid = GetComponent<Grid>();
        roomBoxCollider = GetComponent<BoxCollider>();
        roomBoxCollider.isTrigger = true;
    }

    private void OnSceneLoaded()
    {
        if (sceneRoom == null) return;

        // setup room box collider
        float height = sceneRoom.Walls[0].Height;
        float width = sceneRoom.Floor.Width;
        float depth = sceneRoom.Floor.Height;

        roomBoxCollider.size = new Vector3(width, depth, height);
        roomBoxCollider.center = new Vector3(0, 0, height/2);
    }
}
