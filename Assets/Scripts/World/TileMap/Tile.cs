using UnityEngine;

public class Tile {

    public enum TerrainType
    {
        DESERT,
        GRASS,
        WATER
    };
    
    TerrainType type;
    float height; //overall tile height
    Vector4 heightPoints; //heights of corners of the square
    float precp; //precipitation

    public Tile(float height)
    {
        this.height = height;
        DeduceType();
    }

    void DeduceType()
    {
        if(height >= 0)
        {
            type = TerrainType.GRASS;
        }
        else
        {
            type = TerrainType.WATER;
        }
    }

    public TerrainType Type
    {
        get { return type; }
        set { type = value; }
    }

    public float Height
    {
        get { return height; }
        set { height = value; }
    }

    public Vector4 HeightPoints
    {
        get { return heightPoints; }
        set { heightPoints = value; }
    }

    public float Precp
    {
        get { return precp; }
        set { precp = value; }
    }
}
