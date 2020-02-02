using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum ArtTools
{
    PaintBrush,
    EyeDrop
}

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

    public float brushScaleModifier = 1f;
    
    private RenderTexture _artWorkTexture;
    private RenderTexture _paintLayer;
    private RenderTexture _noiseLayer;

    private Camera _camera;

    public Color paintColor;

    private Vector2 _prevMousePosition;
    private float _prevRotation;

    public bool CanDraw = false;

    public ArtTools activeTool = ArtTools.PaintBrush;

    private AudioSource AudioSource;
    public AudioClip[] ScrapeAudioClips;
    public AudioClip EyeDropSound;
    public AudioClip[] RandomGuardNoisesWhilePainting;
    
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
        
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", _artWorkTexture);

        paintShaderKernel = PaintShader.FindKernel("CSMain");
        
        
        paintColor = Color.white;

        AudioSource = GetComponent<AudioSource>();
    }

    [ContextMenu("CorrectSize")]
    void correctSize()
    {
        transform.localScale = new Vector3(OriginalArtWork.width * scale,OriginalArtWork.height * scale, 1f);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (CanDraw)
        {
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            bool didHit = Physics.Raycast(ray, out hit);
            
            if (didHit)
            {
                if (hit.transform == transform)
                {
                    var tools = GameObject.FindWithTag("Tools").GetComponent<Tools>();

                    tools.canSee = true;
                    tools.SetActiveTool(activeTool);
                    tools.SetActiveColor(paintColor);
                }
            }
            else
            {
                var tools = GameObject.FindWithTag("Tools").GetComponent<Tools>();
                tools.canSee = false;
            }

            if (Input.GetMouseButton(0) && activeTool == ArtTools.PaintBrush)
            {
                if (didHit)
                {
                    if (hit.transform == transform)
                    {
                        if (Random.Range(0f, 1f) < 0.0025f)
                        {
                            var audioSource = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
                            
                            audioSource.PlayOneShot(RandomGuardNoisesWhilePainting[Random.Range(0, RandomGuardNoisesWhilePainting.Length)]);
                        }
                        
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
                            
                            if(!AudioSource.isPlaying)
                                AudioSource.PlayOneShot(ScrapeAudioClips[Random.Range(0, ScrapeAudioClips.Length)]);
                        }
                        else
                        {
                            rot = _prevRotation;
                        }

                        Debug.Log(rot);

                        PaintShader.SetTexture(paintShaderKernel, "Result", _paintLayer);
                        PaintShader.SetFloats("drawColor", paintColor.r, paintColor.g, paintColor.b);
                        PaintShader.SetFloats("drawPosition", x, y);
                        PaintShader.SetFloat("brushScale", brushScale * brushScaleModifier);
                        PaintShader.SetFloat("brushRotation", rot);
                        PaintShader.SetTexture(paintShaderKernel, "_Brush", brushTexture);
                        PaintShader.Dispatch(paintShaderKernel, OriginalArtWork.width / 8, OriginalArtWork.height / 8,
                            1);

                        _prevMousePosition = Input.mousePosition;
                        _prevRotation = rot;
                    }
                }
            }

            if (Input.GetMouseButtonDown(1) || (Input.GetMouseButtonDown(0) && activeTool == ArtTools.EyeDrop))
            {
                if (didHit)
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

                        y /= (float) OriginalArtWork.height / OriginalArtWork.width;
                        y /= 2.0f;
                        x /= 2.0f;
                        x += 0.5f;
                        y += 0.5f;
                        x *= OriginalArtWork.width;
                        y *= OriginalArtWork.height;

                        var texture = new Texture2D(OriginalArtWork.width, OriginalArtWork.height);

                        RenderTexture.active = _artWorkTexture;
                        texture.ReadPixels(new Rect(0, 0, OriginalArtWork.width, OriginalArtWork.height), 0, 0);
                        texture.Apply();
                        RenderTexture.active = null;

                        paintColor = texture.GetPixel((int) x, (int) y);

                        Destroy(texture);
                        
                        if(!AudioSource.isPlaying)
                            AudioSource.PlayOneShot(EyeDropSound);
                    }
                }
            }
        }

        Shader.SetTexture(shaderKernel, "Result", _artWorkTexture);
        Shader.SetTexture(shaderKernel, "_ArtWork", OriginalArtWork);
        Shader.SetTexture(shaderKernel, "_Paint", _paintLayer);
        Shader.SetTexture(shaderKernel, "_Noise", _noiseLayer);
        Shader.Dispatch(shaderKernel, OriginalArtWork.width / 8, OriginalArtWork.height / 8, 1);
    }

    public Texture GetArtwork()
    {
        return _artWorkTexture;
    }
}
