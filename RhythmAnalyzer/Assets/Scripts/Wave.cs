using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{

    public int Width = 0;
    public int Length = 0;
    public float HeightScale;
    public float SpeedScale;


    private float Height;
    public float Lerp;

    private Terrain ThisTerrain;
    private float offset = 0;
    private float[,] map;


    // Use this for initialization
    void Start()
    {
        ThisTerrain = this.GetComponent<Terrain>();
        this.transform.position = new Vector3(-Width / 2, 0, -Length / 2);
        Height = 0;
        Lerp = 0.25f;
        map = new float[Width, Length];
    }

    // Update is called once per frame
    void Update()
    {

        Height = Mathf.Lerp(Height, AudioVisualizer.Height * 10, Lerp) * HeightScale;
        Lerp = Mathf.Lerp(Lerp, AudioVisualizer.Scale / 10 + 2, 0.3f) * SpeedScale;
        ThisTerrain.terrainData = GenerateTerrain(ThisTerrain.terrainData);
        offset += Time.deltaTime /2;
    }

    TerrainData GenerateTerrain(TerrainData ThisTerrainData)
    {
        ThisTerrainData.heightmapResolution = Width + 1;

        ThisTerrainData.size = new Vector3(Width, 15, Length);

        map = GenerateHeight(map);

        ThisTerrainData.SetHeights(0, 0, map);

        return ThisTerrainData;
    }

    float[,] GenerateHeight(float[,] map)
    {
        float[,] Result = map;

        for (int x = 1; x < 2; x++)
        {
            for (int y = 1; y < Length; y++)
            {
                Result[x, y] = CalculateHeight() + Mathf.Lerp(Result[x, y], Height, Lerp);
            }
        }

        for (int x = Length - 2; x >= 2; x--)
        {
            for (int y = 1; y < Length; y++)
            {
                Result[x, y] = Result[x - 1, y];
            }
        }

        return Result;
    }

    float CalculateHeight()
    {
        float x1 =  offset;

        return Mathf.PerlinNoise(x1, 10) / 10;
    }
}
