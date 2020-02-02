using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Serialization;

public class ArtWorkShaderDispatcher : MonoBehaviour
{
    public ComputeShader Shader;
    private int shaderKernel;
    
    public ComputeShader PaintShader;
    private int paintShaderKernel;
    
    public CreateNoiseTexture noiseGenerator;
    public Texture2D OriginalArtWork;
    public float scale;

    public Texture2D brushTexture;
    public float brushScale;
    
    private RenderTexture _artWorkTexture;
    private RenderTexture _paintLayer;
    private RenderTexture _noiseLayer;

    private Camera _camera;

    private Color _paintColor;

    private Vector2 _prevMousePosition;
    private float _prevRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;

        _noiseLayer = noiseGenerator.CreateTexture();
        
        _artWorkTexture = new RenderTexture(OriginalArtWork.width, OriginalArtWork.height, 0)
        {
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp,
            enableRandomWrite = true,
            
        };
        _artWorkTexture.Create();
        
        _paintLayer = new RenderTexture(OriginalArtWork.width, OriginalArtWork.height, 0)
        {
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp,
            enableRandomWrite = true,
            
        };
        _paintLayer.Create();
        
        shaderKernel = Shader.FindKernel("CSMain");
        
        Shader.SetTexture(shaderKernel, "Result", _artWorkTexture);
        Shader.SetTexture(shaderKernel, "_ArtWork", OriginalArtWork);
        Shader.SetTexture(shaderKernel, "_Paint", _paintLayer);
        Shader.SetTexture(shaderKernel, "_Noise", _noiseLayer);
        
        transform.localScale = new Vector3(OriginalArtWork.width * scale,OriginalArtWork.height * scale, 1f);
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", _artWorkTexture);

        paintShaderKernel = PaintShader.FindKernel("CSMain");
        PaintShader.SetTexture(paintShaderKernel, "Result", _paintLayer);
        
        
        _paintColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
            _paintColor = Color.red;
        
        if(Input.GetKeyDown(KeyCode.Alpha2))
            _paintColor = Color.green;
        
        if(Input.GetKeyDown(KeyCode.Alpha3))
            _paintColor = Color.blue;
        
        if(Input.GetKeyDown(KeyCode.Alpha4))
            _paintColor = Color.white;
        
        if(Input.GetKeyDown(KeyCode.Alpha5))
            _paintColor = Color.black;
        
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    var point = hit.point;
                    point.z = 0;

                    var center = transform.position;
                    center.z = 0;

                    var right = transform.right * 2.0f;
                    var up = transform.up * 2.0f;

                    var x = Vector3.Dot(right, point - center) / transform.localScale.x;
                    var y = Vector3.Dot(up, point - center) / transform.localScale.x;

                    
                    float rot;
                    
                    Debug.Log(Vector2.Distance(_prevMousePosition, Input.mousePosition) > 1f);
                    
                    if (Vector2.Distance(_prevMousePosition, Input.mousePosition) > 1f)
                    {
                        rot = Mathf.Atan2(Input.mousePosition.y - _prevMousePosition.y,
                            Input.mousePosition.x - _prevMousePosition.x);
                    }
                    else
                    {
                        rot = _prevRotation;
                    }
                    
                    Debug.Log(rot);
                    
                    PaintShader.SetFloats("drawColor", _paintColor.r, _paintColor.g, _paintColor.b);
                    PaintShader.SetFloats("drawPosition", x, y);
                    PaintShader.SetFloat("brushScale", brushScale);
                    PaintShader.SetFloat("brushRotation", rot);
                    PaintShader.SetTexture(paintShaderKernel, "_Brush", brushTexture);
                    PaintShader.Dispatch(paintShaderKernel, OriginalArtWork.width / 8, OriginalArtWork.height / 8, 1);

                    _prevMousePosition = Input.mousePosition;
                    _prevRotation = rot;
                    
                    
                }
            }
        }

        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    var point = hit.point;
                    point.z = 0;

                    var center = transform.position;
                    center.z = 0;

                    var right = transform.right * 2.0f;
                    var up = transform.up * 2.0f;

                    var x = Vector3.Dot(right, point - center) / transform.localScale.x;
                    var y = Vector3.Dot(up, point - center) / transform.localScale.x;

                    y /= OriginalArtWork.height / OriginalArtWork.width;
                    y /= 2.0f;
                    x /= 2.0f;
                    x += 0.5f;
                    y += 0.5f;
                    x *= OriginalArtWork.width;
                    y *= OriginalArtWork.height;
                    
                    var texture = new Texture2D(OriginalArtWork.width, OriginalArtWork.height);
                    
                    RenderTexture.active = _artWorkTexture;
                    texture.ReadPixels (new Rect (0, 0, OriginalArtWork.width, OriginalArtWork.height), 0, 0);
                    texture.Apply ();
                    RenderTexture.active = null;

                    _paintColor = texture.GetPixel((int) x, (int) y);
                    
                    Destroy(texture);
                }
            }
        }

        Shader.Dispatch(shaderKernel, OriginalArtWork.width / 8, OriginalArtWork.height / 8, 1);
    }
}
