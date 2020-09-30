using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource[] sounds;

    void Start()
    {
        sounds = gameObject.GetComponents<AudioSource>();
    }

    public void PlayExplosion()
    {
        sounds[0].Play();
    }

    public void PlayDash()
    {
        sounds[1].Play();
    }

    public void PlaySlash()
    {
        sounds[2].Play();
    }

    public void PlayLand()
    {
        sounds[3].Play();
    }
}
