using UnityEngine;

public class Tile {

    public enum Type
    {
        DESERT,
        GRASS,
        WATER
    };

    bool isVisible_;
    Type type_;
    public Type type
    {
        get { return type_; }
        set { type_ = value; }
    }

    float height_; //overall tile height
    public float Height
    {
        get { return height_; }
        set { height_ = value; }
    }

    Vector4 heightPoints_; //heights of corners of the square
    public Vector4 heightPoints
    {
        get { return heightPoints_; }
        set { heightPoints_ = value; }
    }

    float precp_; //precipitation
    public float precp
    {
        get { return precp_; }
        set { precp_ = value; }
    }

    void deduceType()
    {
        if(height_ >= 0)
        {
            type_ = Type.GRASS;
        }
        else
        {
            type_ = Type.WATER;
        }
    }

    public Tile(float height)
    {
        isVisible_ = true;
        height_ = height;
        deduceType();
    }
}
