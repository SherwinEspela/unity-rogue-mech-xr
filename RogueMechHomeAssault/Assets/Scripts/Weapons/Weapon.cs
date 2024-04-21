using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] AudioSource sfxShoot;
    [SerializeField] AudioClip clipShoot;

    public void Shoot()
    {
        if (sfxShoot && clipShoot)
        {
            Debug.Log("Shoot.......");
            sfxShoot.PlayOneShot(clipShoot);
        }
    }
}
