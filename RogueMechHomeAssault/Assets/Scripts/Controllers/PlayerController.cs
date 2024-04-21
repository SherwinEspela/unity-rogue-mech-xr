using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.BuildingBlocks;

public class PlayerController : MonoBehaviour
{
    [SerializeField] ControllerButtonsMapper controllerButtonMapper;
    [SerializeField] Weapon weaponRightHand;

    private void Start()
    {
        controllerButtonMapper.ButtonClickActions[0].Callback.AddListener(OnRightControllerButtonIndexClicked);   
    }

    private void OnRightControllerButtonIndexClicked()
    {
        if (weaponRightHand)
        {
            weaponRightHand.Shoot();
        }
    }
}
