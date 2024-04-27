using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] AudioSource sfxImpact;
    [SerializeField] AudioClip audioClipImpact;
    [SerializeField] AudioClip audioClipDestroyed;
    [SerializeField] int healthMax = 100;

    private int health;

    private const int HIT_DAMAGE = 10;

    private void Start()
    {
        health = healthMax;
    }

    public void Damage()
    {
        health -= HIT_DAMAGE;
        if (health <= 0)
        {
            sfxImpact.PlayOneShot(audioClipDestroyed);
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        } else
        {
            sfxImpact.PlayOneShot(audioClipImpact);
        }
    }
}
