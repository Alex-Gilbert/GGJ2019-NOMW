using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNoiseTexture : MonoBehaviour
{
    public ComputeShader GenerateNoiseShader;
    public ComputeShader DirectionalBlurShader;
    private int generateNoiseKernel;
    private int directionalBlurKernel;

    private RenderTexture output;
    public Texture2D[] grungeMaps;
    public Texture2D[] dirtMaps;
    public Texture2D[] cellsMaps;
    public Texture2D[] noiseMaps;

    public float NoisePosition;
    public float NoiseContrast;
    
    public float GrungePosition;
    public float GrungeContrast;
    
    public RenderTexture CreateTexture()
    {
        generateNoiseKernel = GenerateNoiseShader.FindKernel("CSMain");
        directionalBlurKernel = DirectionalBlurShader.FindKernel("CSMain");

        output = new RenderTexture(1024, 1024, 0)
        {
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp,
            enableRandomWrite = true,
            
        };
        output.Create();
        
        GenerateNoiseShader.SetTexture(generateNoiseKernel, "Result", output);

        var g = grungeMaps[Random.Range(0, grungeMaps.Length)];
        GenerateNoiseShader.SetTexture(generateNoiseKernel, "GrungeMap", grungeMaps[Random.Range(0, grungeMaps.Length)]);
        GenerateNoiseShader.SetTexture(generateNoiseKernel, "DirtMap", dirtMaps[Random.Range(0, dirtMaps.Length)]);
        GenerateNoiseShader.SetTexture(generateNoiseKernel, "CellsMap", cellsMaps[Random.Range(0, cellsMaps.Length)]);
        GenerateNoiseShader.SetTexture(generateNoiseKernel, "PerlinNoise", noiseMaps[Random.Range(0, noiseMaps.Length)]);
        
        GenerateNoiseShader.SetFloat("grungeContrast", GrungeContrast);
        GenerateNoiseShader.SetFloat("grungeContrastPosition", GrungePosition);
        
        GenerateNoiseShader.SetFloat("noiseContrast", NoiseContrast);
        GenerateNoiseShader.SetFloat("noiseContrastPosition", NoisePosition);
        
        GenerateNoiseShader.Dispatch(generateNoiseKernel, output.width/8, output.height/8, 1);

        return output;
    }

    void Dispose()
    {
        
    }
}
