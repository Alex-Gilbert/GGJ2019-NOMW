using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exhibit : MonoBehaviour
{
    public ArtPiece ArtPiece;

    private MoveScript moveScript;

    private void Start()
    {
        moveScript = GameObject.FindWithTag("Player").GetComponent<MoveScript>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        moveScript.EnableArrow(ArtPiece);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        moveScript.DisableArow();
    }
}
