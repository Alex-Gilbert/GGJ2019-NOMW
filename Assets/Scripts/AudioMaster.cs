using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMaster : MonoBehaviour
{
    public AudioSource LastMinute;
    public AudioSource GameOver;

    private Animator animator;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);

        animator = GetComponent<Animator>();
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
