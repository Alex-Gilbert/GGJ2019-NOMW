using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class GetTextureFromTag : MonoBehaviour
{
    public string tag;
    
    // Start is called before the first frame update
    void Start()
    {
        var t = GameObject.FindWithTag(tag);
        if (t != null)
        {
            var a = t.GetComponent<ArtPiece>();
            GetComponent<Renderer>().material.SetTexture("_MainTex", a.GetPainting());
        }
    }

    public void SaveImage()
    {
        var t = GameObject.FindWithTag(tag);
        if (t != null)
        {
            var a = t.GetComponent<ArtPiece>();

            var aw = a.GetPainting() as RenderTexture;
            
            var tex = new Texture2D(aw.width, aw.height);
            
            RenderTexture.active = aw;
            tex.ReadPixels(new Rect(0, 0, aw.width, aw.height), 0, 0);
            tex.Apply();
            RenderTexture.active = null;
            
            // Encode texture into PNG
            byte[] bytes = tex.EncodeToPNG();
            Object.Destroy(tex);

            // For testing purposes, also write to a file in the project folder
            File.WriteAllBytes(Application.dataPath + $"/../{tag}.png", bytes);
        }
    }
}
