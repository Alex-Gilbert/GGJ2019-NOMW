using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtPiece : MonoBehaviour
{
    public Transform CameraTarget;
    public ArtWorkShaderDispatcher ArtWorkShaderDispatcher;

    public void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void Done()
    {
        foreach (Transform child in transform)
        {
            if (child != transform){
                child.gameObject.SetActive(false);
            }
        }
    }
    
    public Texture GetPainting()
    {
        return ArtWorkShaderDispatcher.GetArtwork();
    }
}
