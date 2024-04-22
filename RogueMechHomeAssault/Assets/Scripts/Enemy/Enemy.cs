using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] AudioSource sfxImpact;
    [SerializeField] AudioClip audioClipImpact;

    public void PlaySfxImpact()
    {
        sfxImpact.PlayOneShot(audioClipImpact);
    }
}
