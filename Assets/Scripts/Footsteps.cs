using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class Footsteps : MonoBehaviour
{
    public AudioClip[] footsteps;
    public AudioClip[] creaks;
    
    private AudioSource _source;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void PlayFootStep()
    {
        var clip = footsteps[Random.Range(0, footsteps.Length)];
        
        _source.PlayOneShot(clip);

        if (Random.Range(0f, 1f) < 0.2f)
        {
            var creak = creaks[Random.Range(0, creaks.Length)];
            _source.PlayOneShot(creak);
        }
    }
}
