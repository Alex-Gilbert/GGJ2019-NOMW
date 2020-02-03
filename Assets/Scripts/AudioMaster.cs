using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMaster : MonoBehaviour
{
    public AudioSource LastMinute;
    public AudioSource GameOver;

    private Animator animator;

    public AudioSource[] allSources;
    public float volume;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);

        animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        for (var i = 0; i < allSources.Length; i++)
        {
            var s = allSources[i];
            s.volume *= volume;
        }
    }

    public void FadeInLayerTwo()
    {
        animator.SetTrigger("FadeInLayerTwo");
    }
    
    public void FadeInLayerThree()
    {
        animator.SetTrigger("FadeInLayerThree");
    }
    
    public void FadeOutLayerThree()
    {
        animator.SetTrigger("FadeOutLayerThree");
    }
    
    public void StartLastMinute()
    {
        animator.SetTrigger("StartLastMinute");
    }
    
    public void StartGameOver()
    {
        animator.SetTrigger("StartGameOver");
    }
    
    public void PlayLastMinute()
    {
        LastMinute.Play();
    }
    
    public void PlayGameOver()
    {
        GameOver.Play();
    }
}
