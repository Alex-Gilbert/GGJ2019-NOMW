using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{
    public SpriteRenderer paintBrush;
    public SpriteRenderer paintBrushFill;
    
    public SpriteRenderer eyeDrop;
    public SpriteRenderer eyeDropFill;
    public SpriteRenderer pointer;

    public ArtTools activeTool = ArtTools.PaintBrush;
    public Color currentColor = Color.white;
    public bool canSee = false;

    // Update is called once per frame
    void Update()
    {
        var mousePosition = Input.mousePosition;
        var pos = transform.position;

        var newPos =
            Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y,
                Camera.main.nearClipPlane));

        transform.position = new Vector3(newPos.x, newPos.y, newPos.z);
        
        if (canSee)
        {
            var vector = transform.position - Camera.main.transform.position;
            var rotation = Mathf.Atan2(vector.y, vector.x);

            transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * rotation);
    
            
            pointer.gameObject.SetActive(false);
            paintBrush.gameObject.SetActive(canSee && activeTool == ArtTools.PaintBrush);
            paintBrushFill.gameObject.SetActive(canSee && activeTool == ArtTools.PaintBrush);

            eyeDrop.gameObject.SetActive(canSee && activeTool == ArtTools.EyeDrop);
            eyeDropFill.gameObject.SetActive(canSee && activeTool == ArtTools.EyeDrop);

            eyeDropFill.color = currentColor;
            paintBrushFill.color = currentColor;
        }
        else
        {
            pointer.gameObject.SetActive(true);
        
            paintBrush.gameObject.SetActive(false);
            paintBrushFill.gameObject.SetActive(false);

            eyeDrop.gameObject.SetActive(false);
            eyeDropFill.gameObject.SetActive(false);
            
            transform.rotation = Quaternion.identity;
        }
    }

    public void SetActiveTool(ArtTools tool)
    {
        activeTool = tool;
    }
}
