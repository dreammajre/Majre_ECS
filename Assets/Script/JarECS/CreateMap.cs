using System.Collections;
using UnityEngine;


public class CreateMap : AbsBehaviour<CreateMap> {


    public static int BlockFaces = 0;
    public static  Texture2D NoisHeightMap;
    int textureWidth = 200;
    int textureHeight = 200;

    float scale1 = 1f;
    float scale2 = 10f;
    float scale3 = 20f;

    float offsetX;
    float offsetY;

    public static CreateMap instance;

     void Awake()
    {
        offsetX = Random.Range(0, 99999);
        offsetY = Random.Range(0, 99999);
        CreateCubeNotEcs.hightMap = GenerateHeightMap();


    }

    public Texture2D GenerateHeightMap() {
        Texture2D texture = new Texture2D(textureWidth, textureHeight);
        for (int x = 0; x < texture.height; x++)
        {
            for (int y = 0; y < texture.width; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
    }

    Color CalculateColor(int x, int y) {
        float xCoord1 = (float)x / textureWidth * scale1 + offsetX;
        float yCoord1 = (float)y / textureHeight * scale1 + offsetY;
        float xCoord2 = (float)x / textureWidth * scale2 + offsetX;
        float yCoord2 = (float)y / textureHeight * scale2 + offsetY;
        float xCoord3 = (float)x / textureWidth * scale3 + offsetX;
        float yCoord3 = (float)y / textureHeight * scale3 + offsetY;

        float sample1 = Mathf.PerlinNoise(xCoord1, yCoord1) / 15;
        float sample2 = Mathf.PerlinNoise(xCoord2, yCoord2) / 15;
        float sample3 = Mathf.PerlinNoise(xCoord3, yCoord3) / 15;
        float colorinfo = sample1 + sample2 + sample3;
     

        return new Color(colorinfo, colorinfo, colorinfo);
    }
}
