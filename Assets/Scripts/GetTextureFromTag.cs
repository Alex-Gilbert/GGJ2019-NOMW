using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
