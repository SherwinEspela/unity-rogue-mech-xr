using UnityEngine;
using Meta.XR.BuildingBlocks;
using Oculus.Interaction;

public class PlayerController : MonoBehaviour
{
    [SerializeField] ControllerButtonsMapper controllerButtonMapper;
    [SerializeField] DistanceGrabInteractor distanceGrabInteractorRightHand;
    [SerializeField] DistanceGrabInteractor distanceGrabInteractorLeftHand;

    private Weapon weaponRightHand;
    private Weapon weaponLeftHand;

    private const string TAG_WEAPON = "Weapon";

    // Methods assigned to ControllerButtonMapper callbacks
    public void OnHandGrab(bool isRight = true)
    {
        if (!distanceGrabInteractorRightHand) return;
        if (!distanceGrabInteractorLeftHand) return;
        var distanceInteractable = isRight ? distanceGrabInteractorRightHand.DistanceInteractable :
                                            distanceGrabInteractorLeftHand.DistanceInteractable;
        if (distanceInteractable.RelativeTo.gameObject.tag.Equals(TAG_WEAPON))
        {
            if (isRight)
            {
                weaponRightHand = distanceInteractable.RelativeTo.gameObject.GetComponent<Weapon>();
            } else
            {
                weaponLeftHand = distanceInteractable.RelativeTo.gameObject.GetComponent<Weapon>();
            }
        }
    }

    public void OnHandShoot(bool isRight = true)
    {
        if (isRight)
        {
            if (weaponRightHand) weaponRightHand.Shoot();
        } else
        {
            if (weaponLeftHand) weaponLeftHand.Shoot();
        }
    }

    public void OnHandRelease(bool isRight = true)
    {
        if (isRight)
        {
            weaponRightHand = null;
        } else
        {
            weaponLeftHand = null;
        }
    }
}
