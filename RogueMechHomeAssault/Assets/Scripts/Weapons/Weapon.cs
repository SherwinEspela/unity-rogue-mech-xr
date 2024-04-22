using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] AudioSource sfxShoot;
    [SerializeField] AudioClip clipShoot;
    [SerializeField] Transform transformRaycastStart;

    private string stringRaycastReport;
    RaycastHit hit;
    bool didHit;

    public void Shoot()
    {
        if (sfxShoot && clipShoot)
        {
            sfxShoot.PlayOneShot(clipShoot);
            HitReport();
        }
    }

    private void HitReport()
    {
        if (didHit)
        {
            if (hit.transform.gameObject.tag.ToLower().Equals("enemy"))
            {
                Debug.Log("Did hit enemy....");
                Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();
                if (enemy)
                {
                    enemy.PlaySfxImpact();
                }
            }
        } else
        {
            Debug.Log("Did NOT hit object....");
        }
        
    }

    void FixedUpdate()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        //int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        //layerMask = ~layerMask;

        didHit = Physics.Raycast(transformRaycastStart.position, transformRaycastStart.TransformDirection(Vector3.forward), out hit, 1000);
        if (didHit)
        {
            Debug.DrawRay(transformRaycastStart.position, transformRaycastStart.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        }
        else
        {
            Debug.DrawRay(transformRaycastStart.position, transformRaycastStart.TransformDirection(Vector3.forward) * 1000, Color.white);
        }
    }
}
