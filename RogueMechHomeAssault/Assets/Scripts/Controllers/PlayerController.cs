using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.BuildingBlocks;
using Oculus.Interaction;

public class PlayerController : MonoBehaviour
{
    [SerializeField] ControllerButtonsMapper controllerButtonMapper;
    [SerializeField] DistanceGrabInteractor distanceGrabInteractorRightHand;
    
    private Weapon weaponRightHand;

    private void Start()
    {
        AddButtonMapperListeners();
    }

    private void AddButtonMapperListeners()
    {
        var actionRightHandGrab = controllerButtonMapper.ButtonClickActions.FindLast((x) => x.Title.Equals("RightHandGrab"));
        actionRightHandGrab.Callback.AddListener(OnRightControllerHandTriggerClicked);

        var actionRightHandShoot = controllerButtonMapper.ButtonClickActions.FindLast((x) => x.Title.Equals("RightHandShoot"));
        actionRightHandShoot.Callback.AddListener(OnRightControllerButtonIndexClicked);
    }

    private void OnRightControllerHandTriggerClicked()
    {
        Debug.Log("OnRightControllerHandTriggerClicked....");

        if (!distanceGrabInteractorRightHand) return;

        var distanceInteractable = distanceGrabInteractorRightHand.DistanceInteractable;
        if (distanceInteractable.RelativeTo.gameObject.tag.Equals("Weapon"))
        {
            weaponRightHand = distanceInteractable.RelativeTo.gameObject.GetComponent<Weapon>();
        }
    }

    private void OnRightControllerButtonIndexClicked()
    {
        Debug.Log("OnRightControllerButtonIndexClicked....");

        if (weaponRightHand)
        {
            weaponRightHand.Shoot();
        }
    }
}
